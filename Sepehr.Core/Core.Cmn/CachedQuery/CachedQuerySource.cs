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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Cmn.CachedQuery
{
    /// <summary>
    /// Possible sources where the result was returned from
    /// </summary>
    public enum ECachedQuerySource
    {
        /// <summary>
        /// Not set
        /// </summary>
        None,

        /// <summary>
        /// Result was returned from cache
        /// </summary>
        FromCache,

        /// <summary>
        /// Result was returned from the query
        /// </summary>
        FromQuery,
    }
}
