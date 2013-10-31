#region License Header
// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/
#endregion
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Thinktecture.IdentityServer
{
    public static class Tracing
    {
        private static TraceSource _ts = new TraceSource("Thinktecture.IdentityServer");

        [DebuggerStepThrough]
        public static void Start(string message)
        {
            TraceEvent(TraceEventType.Start, message);
        }

        [DebuggerStepThrough]
        public static void Stop(string message)
        {
            TraceEvent(TraceEventType.Stop, message);
        }

        [DebuggerStepThrough]
        public static void Information(string message)
        {
            TraceEvent(TraceEventType.Information, message);
        }

        [DebuggerStepThrough]
        public static void InformationFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Information, message, objects);
        }

        [DebuggerStepThrough]
        public static void Warning(string message)
        {
            TraceEvent(TraceEventType.Warning, message);
        }

        [DebuggerStepThrough]
        public static void WarningFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Warning, message, objects);
        }

        [DebuggerStepThrough]
        public static void Error(string message)
        {
            TraceEvent(TraceEventType.Error, message);
        }

        [DebuggerStepThrough]
        public static void ErrorVerbose(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            TraceEventFormat(TraceEventType.Error, "{0}\n\nMethod: {1}\nFilename: {2}\nLine number: {3}", message, memberName, filePath, lineNumber);
        }

        [DebuggerStepThrough]
        public static void ErrorFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Error, message, objects);
        }

        [DebuggerStepThrough]
        public static void Verbose(string message)
        {
            TraceEvent(TraceEventType.Verbose, message);
        }

        [DebuggerStepThrough]
        public static void VerboseFormat(string message, params object[] objects)
        {
            TraceEventFormat(TraceEventType.Verbose, message, objects);
        }

        [DebuggerStepThrough]
        public static void Transfer(string message, Guid activity)
        {
            _ts.TraceTransfer(0, message, activity);
        }

        [DebuggerStepThrough]
        public static void TraceEventFormat(TraceEventType type, string message, params object[] objects)
        {
            var format = string.Format(message, objects);
            TraceEvent(type, format);
        }

        [DebuggerStepThrough]
        public static void TraceEvent(TraceEventType type, string message)
        {
            if (Trace.CorrelationManager.ActivityId == Guid.Empty)
            {
                Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            }

            _ts.TraceEvent(type, 0, message);
        }
    }
}