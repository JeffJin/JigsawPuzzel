using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MediaJigsaw.Infrastructure
{
    public class WrappingStream : Stream
    {
        // Fields
        private Stream _mStreamBase;

        // Methods
        public WrappingStream(Stream streamBase)
        {
            if (streamBase == null)
            {
                throw new ArgumentNullException("streamBase");
            }
            this._mStreamBase = streamBase;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback,
                                               object state)
        {
            this.ThrowIfDisposed();
            return this._mStreamBase.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback,
                                                object state)
        {
            this.ThrowIfDisposed();
            return this._mStreamBase.BeginWrite(buffer, offset, count, callback, state);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._mStreamBase = null;
            }
            base.Dispose(disposing);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            this.ThrowIfDisposed();
            return this._mStreamBase.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            this.ThrowIfDisposed();
            this._mStreamBase.EndWrite(asyncResult);
        }

        public override void Flush()
        {
            this.ThrowIfDisposed();
            this._mStreamBase.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            this.ThrowIfDisposed();
            return this._mStreamBase.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            this.ThrowIfDisposed();
            return this._mStreamBase.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            this.ThrowIfDisposed();
            return this._mStreamBase.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.ThrowIfDisposed();
            this._mStreamBase.SetLength(value);
        }

        private void ThrowIfDisposed()
        {
            if (this._mStreamBase == null)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.ThrowIfDisposed();
            this._mStreamBase.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            this.ThrowIfDisposed();
            this._mStreamBase.WriteByte(value);
        }

        // Properties
        public override bool CanRead
        {
            get { return ((this._mStreamBase != null) && this._mStreamBase.CanRead); }
        }

        public override bool CanSeek
        {
            get { return ((this._mStreamBase != null) && this._mStreamBase.CanSeek); }
        }

        public override bool CanWrite
        {
            get { return ((this._mStreamBase != null) && this._mStreamBase.CanWrite); }
        }

        public override long Length
        {
            get
            {
                this.ThrowIfDisposed();
                return this._mStreamBase.Length;
            }
        }

        public override long Position
        {
            get
            {
                this.ThrowIfDisposed();
                return this._mStreamBase.Position;
            }
            set
            {
                this.ThrowIfDisposed();
                this._mStreamBase.Position = value;
            }
        }

        protected Stream WrappedStream
        {
            get { return this._mStreamBase; }
        }
    }

}
