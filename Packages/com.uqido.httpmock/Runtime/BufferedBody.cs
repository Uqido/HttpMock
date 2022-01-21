using System;
using System.Collections.Generic;
using System.Text;
using Kayak;

namespace HttpMock
{
	class BufferedBody : IResponse
	{
		readonly Func<byte[]> dataFunc;

		public BufferedBody(string data) : this(data, Encoding.UTF8) { }
		public BufferedBody(string data, Encoding encoding) : this(encoding.GetBytes(data)) { }
		public BufferedBody(byte[] data) : this(() => data) { }
		public BufferedBody(Func<string> data) : this(() => Encoding.UTF8.GetBytes(data())) { }
		public BufferedBody(Func<byte[]> data)
		{
			this.dataFunc = data;
		}

		private int _length = 0;
		
		public Func<int> Length => () => _length;

		public IDisposable Connect(IDataConsumer channel)
		{
			// null continuation, consumer must swallow the data immediately.
			var data = dataFunc();
			_length = data.Length;
			var bytes = new ArraySegment<byte>(data);
			channel.OnData(bytes, null);
			channel.OnEnd();
			return null;
		}

		public void SetRequestHeaders(IDictionary<string, string> requestHeaders) {
		}
	}

	class NoBody : IResponse
	{
		
		
		public IDisposable Connect(IDataConsumer channel) {
			return null;
		}

		public void SetRequestHeaders(IDictionary<string, string> requestHeaders) {
		}

		public Func<int> Length { get; } = () => 0;
	}
}