﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace FASTER.core
{
    internal sealed class LogCompactionFunctions<Key, Value, Input, Output, Context, Functions> : IFunctions<Key, Value, Input, Output, Context>
        where Functions : IFunctions<Key, Value, Input, Output, Context>

    {
        readonly Functions _functions;

        public LogCompactionFunctions(Functions functions)
        {
            _functions = functions;
        }

        public bool SupportsPostOperations => false;

        public void CheckpointCompletionCallback(string sessionId, CommitPoint commitPoint) { }

        /// <summary>
        /// No reads during compaction
        /// </summary>
        public bool ConcurrentReader(ref Key key, ref Input input, ref Value value, ref Output dst, ref RecordInfo recordInfo, long address) => true;

        public void PostSingleDeleter(ref Key key, ref RecordInfo recordInfo, long address) { }

        /// <summary>
        /// No ConcurrentDeleter needed for compaction
        /// </summary>
        public bool ConcurrentDeleter(ref Key key, ref Value value, ref RecordInfo recordInfo, long address) => true;

        /// <summary>
        /// For compaction, we never perform concurrent writes as rolled over data defers to
        /// newly inserted data for the same key.
        /// </summary>
        public bool ConcurrentWriter(ref Key key, ref Input input, ref Value src, ref Value dst, ref Output output, ref RecordInfo recordInfo, long address) => true;

        public void CopyUpdater(ref Key key, ref Input input, ref Value oldValue, ref Value newValue, ref Output output, ref RecordInfo recordInfo, long address) { }
        
        public bool PostCopyUpdater(ref Key key, ref Input input, ref Value oldValue, ref Value newValue, ref Output output, ref RecordInfo recordInfo, long address) => true;

        public void DeleteCompletionCallback(ref Key key, Context ctx) { }

        public void InitialUpdater(ref Key key, ref Input input, ref Value value, ref Output output, ref RecordInfo recordInfo, long address) { }
        public void PostInitialUpdater(ref Key key, ref Input input, ref Value value, ref Output output, ref RecordInfo recordInfo, long address) { }

        public bool InPlaceUpdater(ref Key key, ref Input input, ref Value value, ref Output output, ref RecordInfo recordInfo, long address) => true;

        public bool NeedInitialUpdate(ref Key key, ref Input input, ref Output output) => true;

        public bool NeedCopyUpdate(ref Key key, ref Input input, ref Value oldValue, ref Output output) => true;

        public void ReadCompletionCallback(ref Key key, ref Input input, ref Output output, Context ctx, Status status, RecordMetadata recordMetadata) { }

        public void RMWCompletionCallback(ref Key key, ref Input input, ref Output output, Context ctx, Status status, RecordMetadata recordMetadata) { }

        /// <summary>
        /// No reads during compaction
        /// </summary>
        public bool SingleReader(ref Key key, ref Input input, ref Value value, ref Output dst, ref RecordInfo recordInfo, long address) => true;

        /// <summary>
        /// Write compacted live value to store
        /// </summary>
        public void SingleWriter(ref Key key, ref Input input, ref Value src, ref Value dst, ref Output output, ref RecordInfo recordInfo, long address) 
            => _functions.SingleWriter(ref key, ref input, ref src, ref dst, ref output, ref recordInfo, address);
        public void PostSingleWriter(ref Key key, ref Input input, ref Value src, ref Value dst, ref Output output, ref RecordInfo recordInfo, long address) { }

        public void UpsertCompletionCallback(ref Key key, ref Input input, ref Value value, Context ctx) { }

        public bool SupportsLocking => false;
        public void LockExclusive(ref RecordInfo recordInfo, ref Key key, ref Value value, ref long lockContext) { }
        public void UnlockExclusive(ref RecordInfo recordInfo, ref Key key, ref Value value, long lockContext) { }
        public bool TryLockExclusive(ref RecordInfo recordInfo, ref Key key, ref Value value, ref long lockContext, int spinCount = 1) => true;
        public void LockShared(ref RecordInfo recordInfo, ref Key key, ref Value value, ref long lockContext) { }
        public bool UnlockShared(ref RecordInfo recordInfo, ref Key key, ref Value value, long lockContext) => true;
        public bool TryLockShared(ref RecordInfo recordInfo, ref Key key, ref Value value, ref long lockContext, int spinCount = 1) => true;
    }
}