using System;
using System.Collections.Generic;

namespace Surging.Core.EventBusKafka
{
    public interface IConsumeConfigurator
    {
        void Configure(List<Type> consumers);

        void Unconfigure(List<Type> consumers);
    }
}