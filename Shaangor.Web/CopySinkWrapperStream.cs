using System;
using System.IO;
using System.Text;

namespace Shaangor.Web
{
	internal sealed class CopySinkWrapperStream:Stream
	{
		private readonly Stream _sink;
		private readonly Stream _copy;
		private readonly Encoding _encoding;

		public event EventHandler BeforeClose;

		internal CopySinkWrapperStream(Stream sink,Encoding encoding)
		{
			_sink=sink;
			_copy=new MemoryStream();
			_encoding=encoding;
		}

		public override bool CanRead
		{
			get { return _sink.CanRead; }
		}

		public override bool CanSeek
		{
			get { return _sink.CanSeek; }
		}

		public override bool CanWrite
		{
			get { return _sink.CanWrite; }
		}

		public override long Length
		{
			get { return _sink.Length; }
		}

		public override long Position
		{
			get { return _sink.Position; }
			set { _sink.Position=_copy.Position=value; }
		}

		public override long Seek(long offset,SeekOrigin direction)
		{
			_copy.Seek(offset,direction);
			return _sink.Seek(offset,direction);
		}

		public override void SetLength(long length)
		{
			_copy.SetLength(length);
			_sink.SetLength(length);
		}

		public override void Close()
		{
			BeforeClose?.Invoke(this,new EventArgs());
			_sink.Close();
			_copy.Close();
		}

		public override void Flush()
		{
			_sink.Flush();
			_copy.Flush();
		}

		public override int Read(byte[] buffer,int offset,int count)
		{
			return _sink.Read(buffer,offset,count);
		}

		public override void Write(byte[] buffer,int offset,int count)
		{
			_copy.Write(buffer,offset,count);
			_sink.Write(buffer,offset,count);
		}

		public string ReadCopyToEnd()
		{
			using(var reader = new StreamReader(_copy,_encoding,true,1024,true))
			{
				var currentPosition=_copy.Position;
				_copy.Position=0L;
				var text=reader.ReadToEnd();
				_copy.Position=currentPosition;
				return text;
			}
		}
	}
}
