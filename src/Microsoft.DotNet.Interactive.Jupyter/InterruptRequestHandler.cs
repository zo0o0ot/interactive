// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using Microsoft.DotNet.Interactive.Events;
using Microsoft.DotNet.Interactive.Jupyter.Protocol;

namespace Microsoft.DotNet.Interactive.Jupyter
{
    public class InterruptRequestHandler : RequestHandlerBase<InterruptRequest>
    {
        public InterruptRequestHandler(Kernel kernel, IScheduler scheduler = null)
            : base(kernel, scheduler ?? CurrentThreadScheduler.Instance)
        {
        }

        protected override void OnKernelEventReceived(KernelEvent @event, JupyterRequestContext context)
        {
        }

        public Task Handle(JupyterRequestContext context)
        {
            if (KernelInvocationContext.Current is { } invocationContext)
            {
                invocationContext.Fail(
                    invocationContext.Command,
                    new OperationCanceledException());
            }

            // reply 
            var interruptReplyPayload = new InterruptReply();

            // send to server
            context.JupyterMessageSender.Send(interruptReplyPayload);

            return Task.CompletedTask;
        }
    }
}