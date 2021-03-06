
// Type: ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream


// Hacked by SystemAce

using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Encryption;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using System;
using System.IO;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
  public class DeflaterOutputStream : Stream
  {
    private bool isStreamOwner_ = true;
    private string password;
    private ICryptoTransform cryptoTransform_;
    protected byte[] AESAuthCode;
    private byte[] buffer_;
    protected Deflater deflater_;
    protected Stream baseOutputStream_;
    private bool isClosed_;
    private static RNGCryptoServiceProvider _aesRnd;

    public bool IsStreamOwner
    {
      get
      {
        return this.isStreamOwner_;
      }
      set
      {
        this.isStreamOwner_ = value;
      }
    }

    public bool CanPatchEntries
    {
      get
      {
        return this.baseOutputStream_.CanSeek;
      }
    }

    public string Password
    {
      get
      {
        return this.password;
      }
      set
      {
        if (value != null && value.Length == 0)
          this.password = (string) null;
        else
          this.password = value;
      }
    }

    public override bool CanRead
    {
      get
      {
        return false;
      }
    }

    public override bool CanSeek
    {
      get
      {
        return false;
      }
    }

    public override bool CanWrite
    {
      get
      {
        return this.baseOutputStream_.CanWrite;
      }
    }

    public override long Length
    {
      get
      {
        return this.baseOutputStream_.Length;
      }
    }

    public override long Position
    {
      get
      {
        return this.baseOutputStream_.Position;
      }
      set
      {
        throw new NotSupportedException("Position property not supported");
      }
    }

    public DeflaterOutputStream(Stream baseOutputStream)
      : this(baseOutputStream, new Deflater(), 512)
    {
    }

    public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater)
      : this(baseOutputStream, deflater, 512)
    {
    }

    public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater, int bufferSize)
    {
      if (baseOutputStream == null)
        throw new ArgumentNullException("baseOutputStream");
      if (!baseOutputStream.CanWrite)
        throw new ArgumentException("Must support writing", "baseOutputStream");
      if (deflater == null)
        throw new ArgumentNullException("deflater");
      if (bufferSize < 512)
        throw new ArgumentOutOfRangeException("bufferSize");
      this.baseOutputStream_ = baseOutputStream;
      this.buffer_ = new byte[bufferSize];
      this.deflater_ = deflater;
    }

    public virtual void Finish()
    {
      this.deflater_.Finish();
      while (!this.deflater_.IsFinished)
      {
        int num = this.deflater_.Deflate(this.buffer_, 0, this.buffer_.Length);
        if (num > 0)
        {
          if (this.cryptoTransform_ != null)
            this.EncryptBlock(this.buffer_, 0, num);
          this.baseOutputStream_.Write(this.buffer_, 0, num);
        }
        else
          break;
      }
      if (!this.deflater_.IsFinished)
        throw new SharpZipBaseException("Can't deflate all input?");
      this.baseOutputStream_.Flush();
      if (this.cryptoTransform_ == null)
        return;
      if (this.cryptoTransform_ is ZipAESTransform)
        this.AESAuthCode = ((ZipAESTransform) this.cryptoTransform_).GetAuthCode();
      this.cryptoTransform_.Dispose();
      this.cryptoTransform_ = (ICryptoTransform) null;
    }

    protected void EncryptBlock(byte[] buffer, int offset, int length)
    {
      this.cryptoTransform_.TransformBlock(buffer, 0, length, buffer, 0);
    }

    protected void InitializePassword(string password)
    {
      this.cryptoTransform_ = new PkzipClassicManaged().CreateEncryptor(PkzipClassic.GenerateKeys(ZipConstants.ConvertToArray(password)), (byte[]) null);
    }

    protected void InitializeAESPassword(ZipEntry entry, string rawPassword, out byte[] salt, out byte[] pwdVerifier)
    {
      salt = new byte[entry.AESSaltLen];
      if (DeflaterOutputStream._aesRnd == null)
        DeflaterOutputStream._aesRnd = new RNGCryptoServiceProvider();
      DeflaterOutputStream._aesRnd.GetBytes(salt);
      int blockSize = entry.AESKeySize / 8;
      this.cryptoTransform_ = (ICryptoTransform) new ZipAESTransform(rawPassword, salt, blockSize, true);
      pwdVerifier = ((ZipAESTransform) this.cryptoTransform_).PwdVerifier;
    }

    protected void Deflate()
    {
      while (!this.deflater_.IsNeedingInput)
      {
        int num = this.deflater_.Deflate(this.buffer_, 0, this.buffer_.Length);
        if (num > 0)
        {
          if (this.cryptoTransform_ != null)
            this.EncryptBlock(this.buffer_, 0, num);
          this.baseOutputStream_.Write(this.buffer_, 0, num);
        }
        else
          break;
      }
      if (!this.deflater_.IsNeedingInput)
        throw new SharpZipBaseException("DeflaterOutputStream can't deflate all input?");
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      throw new NotSupportedException("DeflaterOutputStream Seek not supported");
    }

    public override void SetLength(long value)
    {
      throw new NotSupportedException("DeflaterOutputStream SetLength not supported");
    }

    public override int ReadByte()
    {
      throw new NotSupportedException("DeflaterOutputStream ReadByte not supported");
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      throw new NotSupportedException("DeflaterOutputStream Read not supported");
    }

    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
    {
      throw new NotSupportedException("DeflaterOutputStream BeginRead not currently supported");
    }

    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
    {
      throw new NotSupportedException("BeginWrite is not supported");
    }

    public override void Flush()
    {
      this.deflater_.Flush();
      this.Deflate();
      this.baseOutputStream_.Flush();
    }

    public override void Close()
    {
      if (this.isClosed_)
        return;
      this.isClosed_ = true;
      try
      {
        this.Finish();
        if (this.cryptoTransform_ == null)
          return;
        this.GetAuthCodeIfAES();
        this.cryptoTransform_.Dispose();
        this.cryptoTransform_ = (ICryptoTransform) null;
      }
      finally
      {
        if (this.isStreamOwner_)
          this.baseOutputStream_.Close();
      }
    }

    private void GetAuthCodeIfAES()
    {
      if (!(this.cryptoTransform_ is ZipAESTransform))
        return;
      this.AESAuthCode = ((ZipAESTransform) this.cryptoTransform_).GetAuthCode();
    }

    public override void WriteByte(byte value)
    {
      this.Write(new byte[1]
      {
        value
      }, 0, 1);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.deflater_.SetInput(buffer, offset, count);
      this.Deflate();
    }
  }
}
