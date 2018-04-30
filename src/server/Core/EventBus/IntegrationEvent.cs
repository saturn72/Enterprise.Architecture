﻿using System;

namespace EventBus
{
    public class IntegrationEvent
    {
        #region ctor
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedOnUtc = DateTime.UtcNow;
        }

        #endregion

        #region Properties

        public Guid Id { get; }
        public DateTime CreatedOnUtc { get; }

        #endregion
    }
}
