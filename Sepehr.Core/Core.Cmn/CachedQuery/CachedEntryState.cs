﻿
// Copyright (c) 2010. Rusanu Consulting LLC  
// http://code.google.com/p/linqtocache/
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

namespace Core.Cmn.CachedQuery
{
    /// <summary>
    /// Cached entry states
    /// </summary>
    public enum ECachedEntryState
    {
        /// <summary>
        /// None: initial state
        /// </summary>
        None,

        /// <summary>
        /// Entry is valid
        /// </summary>
        Valid,

        /// <summary>
        /// Entry was invalidated
        /// </summary>
        Invalid
    };
}