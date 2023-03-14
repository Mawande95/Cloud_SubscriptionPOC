using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Entities
{
    public class Validation: IDisposable
    {
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        [NotMapped]
        public bool Error { get; set; } = false;
        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public int ErrorNumber { get; set; }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }

        public enum ErroType
        {
            Default,
            Top3Learner
        }
    }
}
