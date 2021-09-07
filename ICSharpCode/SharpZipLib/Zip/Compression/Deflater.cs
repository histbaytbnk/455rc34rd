﻿
// Type: ICSharpCode.SharpZipLib.Zip.Compression.Deflater


// Hacked by SystemAce

using System;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
  public class Deflater
  {
    public const int BEST_COMPRESSION = 9;
    public const int BEST_SPEED = 1;
    public const int DEFAULT_COMPRESSION = -1;
    public const int NO_COMPRESSION = 0;
    public const int DEFLATED = 8;
    private const int IS_SETDICT = 1;
    private const int IS_FLUSHING = 4;
    private const int IS_FINISHING = 8;
    private const int INIT_STATE = 0;
    private const int SETDICT_STATE = 1;
    private const int BUSY_STATE = 16;
    private const int FLUSHING_STATE = 20;
    private const int FINISHING_STATE = 28;
    private const int FINISHED_STATE = 30;
    private const int CLOSED_STATE = 127;
    private int level;
    private bool noZlibHeaderOrFooter;
    private int state;
    private long totalOut;
    private DeflaterPending pending;
    private DeflaterEngine engine;

    public int Adler
    {
      get
      {
        return this.engine.Adler;
      }
    }

    public long TotalIn
    {
      get
      {
        return this.engine.TotalIn;
      }
    }

    public long TotalOut
    {
      get
      {
        return this.totalOut;
      }
    }

    public bool IsFinished
    {
      get
      {
        if (this.state == 30)
          return this.pending.IsFlushed;
        return false;
      }
    }

    public bool IsNeedingInput
    {
      get
      {
        return this.engine.NeedsInput();
      }
    }

    public Deflater()
      : this(-1, false)
    {
    }

    public Deflater(int level)
      : this(level, false)
    {
    }

    public Deflater(int level, bool noZlibHeaderOrFooter)
    {
      if (level == -1)
        level = 6;
      else if (level < 0 || level > 9)
        throw new ArgumentOutOfRangeException("level");
      this.pending = new DeflaterPending();
      this.engine = new DeflaterEngine(this.pending);
      this.noZlibHeaderOrFooter = noZlibHeaderOrFooter;
      this.SetStrategy(DeflateStrategy.Default);
      this.SetLevel(level);
      this.Reset();
    }

    public void Reset()
    {
      this.state = this.noZlibHeaderOrFooter ? 16 : 0;
      this.totalOut = 0L;
      this.pending.Reset();
      this.engine.Reset();
    }

    public void Flush()
    {
      this.state |= 4;
    }

    public void Finish()
    {
      this.state |= 12;
    }

    public void SetInput(byte[] input)
    {
      this.SetInput(input, 0, input.Length);
    }

    public void SetInput(byte[] input, int offset, int count)
    {
      if ((this.state & 8) != 0)
        throw new InvalidOperationException("Finish() already called");
      this.engine.SetInput(input, offset, count);
    }

    public void SetLevel(int level)
    {
      if (level == -1)
        level = 6;
      else if (level < 0 || level > 9)
        throw new ArgumentOutOfRangeException("level");
      if (this.level == level)
        return;
      this.level = level;
      this.engine.SetLevel(level);
    }

    public int GetLevel()
    {
      return this.level;
    }

    public void SetStrategy(DeflateStrategy strategy)
    {
      this.engine.Strategy = strategy;
    }

    public int Deflate(byte[] output)
    {
      return this.Deflate(output, 0, output.Length);
    }

    public int Deflate(byte[] output, int offset, int length)
    {
      int num1 = length;
      if (this.state == (int) sbyte.MaxValue)
        throw new InvalidOperationException("Deflater closed");
      if (this.state < 16)
      {
        int num2 = 30720;
        int num3 = this.level - 1 >> 1;
        if (num3 < 0 || num3 > 3)
          num3 = 3;
        int num4 = num2 | num3 << 6;
        if ((this.state & 1) != 0)
          num4 |= 32;
        this.pending.WriteShortMSB(num4 + (31 - num4 % 31));
        if ((this.state & 1) != 0)
        {
          int adler = this.engine.Adler;
          this.engine.ResetAdler();
          this.pending.WriteShortMSB(adler >> 16);
          this.pending.WriteShortMSB(adler & (int) ushort.MaxValue);
        }
        this.state = 16 | this.state & 12;
      }
      while (true)
      {
        do
        {
          do
          {
            int num2 = this.pending.Flush(output, offset, length);
            offset += num2;
            this.totalOut += (long) num2;
            length -= num2;
            if (length == 0 || this.state == 30)
              goto label_24;
          }
          while (this.engine.Deflate((this.state & 4) != 0, (this.state & 8) != 0));
          if (this.state == 16)
            return num1 - length;
          if (this.state == 20)
          {
            if (this.level != 0)
            {
              int num2 = 8 + (-this.pending.BitCount & 7);
              while (num2 > 0)
              {
                this.pending.WriteBits(2, 10);
                num2 -= 10;
              }
            }
            this.state = 16;
          }
        }
        while (this.state != 28);
        this.pending.AlignToByte();
        if (!this.noZlibHeaderOrFooter)
        {
          int adler = this.engine.Adler;
          this.pending.WriteShortMSB(adler >> 16);
          this.pending.WriteShortMSB(adler & (int) ushort.MaxValue);
        }
        this.state = 30;
      }
label_24:
      return num1 - length;
    }

    public void SetDictionary(byte[] dictionary)
    {
      this.SetDictionary(dictionary, 0, dictionary.Length);
    }

    public void SetDictionary(byte[] dictionary, int index, int count)
    {
      if (this.state != 0)
        throw new InvalidOperationException();
      this.state = 1;
      this.engine.SetDictionary(dictionary, index, count);
    }
  }
}
