using System.IO;
using System.Text;

namespace Shaangor.Web
{
	internal sealed class CopySinkWrapperFilter:Stream
	{
		private Stream _sink;
		private Stream _copy;

		public AccessLogItem AccessLogItem { get; private set; }

		internal CopySinkWrapperFilter(Stream sink,AccessLogItem item)
		{
			_sink=sink;
			AccessLogItem=item;
			_copy=new MemoryStream();
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
			set { _sink.Position=value; }
		}

		public override long Seek(long offset,SeekOrigin direction)
		{
			return _sink.Seek(offset,direction);
		}

		public override void SetLength(long length)
		{
			_sink.SetLength(length);
		}

		public override void Close()
		{
			_sink.Close();
			_copy.Close();
		}

		public override void Flush()
		{
			_sink.Flush();
			AccessLogItem.ResponseBody=GetContents(new UTF8Encoding(false));
			//TODO: log to db here
		}

		public override int Read(byte[] buffer,int offset,int count)
		{
			return _sink.Read(buffer,offset,count);
		}

		public override void Write(byte[] buffer,int offset,int count)
		{
			_copy.Write(buffer,0,count);
			_sink.Write(buffer,offset,count);
		}

		public string GetContents(Encoding encoding)
		{
			var buffer=new byte[_copy.Length];
			_copy.Position=0;
			_copy.Read(buffer,0,buffer.Length);
			return encoding.GetString(buffer,0,buffer.Length);
		}
	}
}
