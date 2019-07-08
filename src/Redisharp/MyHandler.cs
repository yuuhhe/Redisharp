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

using DotNetty.Codecs.Redis.Messages;
using DotNetty.Transport.Channels;
using System;

namespace Yuuhhe.Redisharp
{
    class MyHandler : ChannelHandlerAdapter
    {
        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);

            IRedisMessage request;
            request = new InlineCommandRedisMessage("HGETALL 0003_1701");
            context.WriteAndFlushAsync(request);
            request = new InlineCommandRedisMessage("INFO");
            context.WriteAndFlushAsync(request);
            request = new InlineCommandRedisMessage("CLUSTER NODES");
            context.WriteAndFlushAsync(request);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            base.ChannelRead(context, message);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            base.ExceptionCaught(context, exception);
        }
    }
}