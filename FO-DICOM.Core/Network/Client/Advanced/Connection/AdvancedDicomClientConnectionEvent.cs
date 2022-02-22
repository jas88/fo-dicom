﻿// Copyright (c) 2012-2021 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).

using System;

namespace FellowOakDicom.Network.Client.Advanced.Connection
{
    /// <summary>
    /// This is an empty marker interface so it is possible to create collections of the various events that occur in our DICOM network communication
    /// </summary>
    public interface IAdvancedDicomClientConnectionEvent
    {
        
    }
    
    /// <summary>
    /// When the TCP connection with the SCP is closed
    /// </summary>
    public class ConnectionClosedConnectionEvent : IAdvancedDicomClientConnectionEvent
    {
        /// <summary>
        /// When the connection closed without an error
        /// </summary>
        public static readonly ConnectionClosedConnectionEvent WithoutException = new ConnectionClosedConnectionEvent();
        
        /// <summary>
        /// When the connection closed with an error
        /// </summary>
        /// <param name="exception">The error that occured while trying to read from or write to the connection</param>
        /// <returns></returns>
        public static ConnectionClosedConnectionEvent WithException(Exception exception) => new ConnectionClosedConnectionEvent(exception);
        
        /// <summary>
        /// (Optional) the exception that occured while trying to read from or write to the connection
        /// </summary>
        public Exception Exception { get; }

        private ConnectionClosedConnectionEvent()
        {
            
        }

        private ConnectionClosedConnectionEvent(Exception exception)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        internal void ThrowException()
        {
            if (Exception != null)
            {
                throw new ConnectionClosedPrematurelyException(Exception);
            }

            throw new ConnectionClosedPrematurelyException();
        }
    }
    
    /// <summary>
    /// When the DICOM association is suddenly aborted
    /// </summary>
    public class DicomAbortedConnectionEvent : IAdvancedDicomClientConnectionEvent
    {
        /// <summary>
        /// Who initiated the ABORT
        /// </summary>
        public DicomAbortSource Source { get; }
        
        /// <summary>
        /// Why the ABORT occurred
        /// </summary>
        public DicomAbortReason Reason { get; }

        /// <summary>
        /// Initializes a new DicomAbortedEvent 
        /// </summary>
        public DicomAbortedConnectionEvent(DicomAbortSource source, DicomAbortReason reason)
        {
            Source = source;
            Reason = reason;
        }
    }
    
    /// <summary>
    /// When the DICOM association is accepted
    /// </summary>
    public class DicomAssociationAcceptedConnectionEvent : IAdvancedDicomClientConnectionEvent
    {
        public DicomAssociation Association { get; }

        /// <summary>
        /// Initializes a new DicomAssociationAcceptedEvent
        /// </summary>
        public DicomAssociationAcceptedConnectionEvent(DicomAssociation association)
        {
            Association = association ?? throw new ArgumentNullException(nameof(association));
        }
    }
    
    /// <summary>
    /// When the DICOM association is rejected
    /// </summary>
    public class DicomAssociationRejectedConnectionEvent : IAdvancedDicomClientConnectionEvent
    {
        /// <summary>
        /// Whether the rejection is permanent or only temporary
        /// </summary>
        public DicomRejectResult Result { get; }
        
        /// <summary>
        /// Who rejected the association
        /// </summary>
        public DicomRejectSource Source { get; }
        
        /// <summary>
        /// Why the association was rejected
        /// </summary>
        public DicomRejectReason Reason { get; }

        /// <summary>
        /// Initializes a new DicomAssociationRejectedEvent
        /// </summary>
        public DicomAssociationRejectedConnectionEvent(DicomRejectResult result, DicomRejectSource source, DicomRejectReason reason)
        {
            Result = result;
            Source = source;
            Reason = reason;
        }
    }

    /// <summary>
    /// When the association is released
    /// </summary>
    public class DicomAssociationReleasedConnectionEvent : IAdvancedDicomClientConnectionEvent
    {
        /// <summary>
        /// An instance of DicomAssociationReleasedEvent, which is a singleton since this event does not have any parameters
        /// </summary>
        public static readonly DicomAssociationReleasedConnectionEvent Instance = new DicomAssociationReleasedConnectionEvent();

        private DicomAssociationReleasedConnectionEvent()
        {
            
        }
    }

    /// <summary>
    /// When a DICOM request is completed and no further responses are expected
    /// </summary>
    public class RequestCompletedConnectionEvent : IAdvancedDicomClientConnectionEvent
    {
        /// <summary>
        /// The original request
        /// </summary>
        public DicomRequest Request { get; }
        
        /// <summary>
        /// The final response
        /// </summary>
        public DicomResponse Response { get; }

        /// <summary>
        /// Initializes a new RequestCompletedEvent
        /// </summary>
        public RequestCompletedConnectionEvent(DicomRequest request, DicomResponse response)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Response = response ?? throw new ArgumentNullException(nameof(response));
        }
    }
    
    /// <summary>
    /// When a DICOM request has been sent to the SCP and is now pending one or more responses 
    /// </summary>
    public class RequestPendingConnectionEvent : IAdvancedDicomClientConnectionEvent
    {
        /// <summary>
        /// The original request
        /// </summary>
        public DicomRequest Request { get; }
        
        /// <summary>
        /// The response that was received from the SCP
        /// </summary>
        public DicomResponse Response { get; }

        /// <summary>
        /// Initializes a new RequestPendingEvent 
        /// </summary>
        public RequestPendingConnectionEvent(DicomRequest request, DicomResponse response)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Response = response ?? throw new ArgumentNullException(nameof(response));
        }
    }
    
    /// <summary>
    /// When a DICOM request times out
    /// </summary>
    public class RequestTimedOutConnectionEvent : IAdvancedDicomClientConnectionEvent
    {
        /// <summary>
        /// The original request
        /// </summary>
        public DicomRequest Request { get; }
        
        /// <summary>
        /// The timeout that elapsed before receiving a response
        /// </summary>
        public TimeSpan Timeout { get; }

        /// <summary>
        /// Initializes a new RequestTimedOutEvent 
        /// </summary>
        public RequestTimedOutConnectionEvent(DicomRequest request, TimeSpan timeout)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Timeout = timeout;
        }
    }

    /// <summary>
    /// When the internal DicomService queue is empty, and a call to SendNextMessage will be required to send more requests
    /// </summary>
    public class SendQueueEmptyConnectionEvent : IAdvancedDicomClientConnectionEvent
    {
        /// <summary>
        /// An instance of SendQueueEmptyEvent, which is a singleton since this event does not have any parameters
        /// </summary>
        public static readonly SendQueueEmptyConnectionEvent Instance = new SendQueueEmptyConnectionEvent();

        private SendQueueEmptyConnectionEvent() {}
    }
}