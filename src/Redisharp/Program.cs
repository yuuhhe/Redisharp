// Copyright 2019 yuuhhe
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using DotNetty.Codecs.Redis;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Net;
using System.Threading.Tasks;

namespace Yuuhhe.Redisharp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var workerGroup = new MultithreadEventLoopGroup();
            var bootstrap = new Bootstrap();
            bootstrap
                .Group(workerGroup)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                //.Option(DotNetty.Transport.Channels.ChannelOption.Allocator, DotNetty.Buffers.PooledByteBufferAllocator.Default)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast("framing-enc", new RedisEncoder());
                    pipeline.AddLast("framing-dec", new RedisDecoder());
                    pipeline.AddLast("decode-bulkString", new RedisBulkStringAggregator());
                    pipeline.AddLast("decode-array", new RedisArrayAggregator());
                    pipeline.AddLast("handler", new MyHandler());
                }));

            var serverChannel = await bootstrap.ConnectAsync(IPAddress.Parse("10.100.45.28"), 8087);
        }
    }
}
