using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class
{
    public abstract class CustomMapTool
    {
        private SharpMap.Forms.MapBox _mapControl;
        private bool _enabled;

        /// <summary>
        /// Event raised when the <see cref="MapControl"/> is about to change.
        /// </summary>
        public event CancelEventHandler MapControlChanging;

        /// <summary>
        /// Event invoker for the <see cref="MapControlChanging"/> event
        /// </summary>
        /// <remarks>Override this method to unwire <see cref="MapBox"/>s events. Don't forget to call <c>base.OnMapControlChanging(cea);</c> to invoke the event.</remarks>
        /// <param name="cea">The cancel event arguments</param>
        protected virtual void OnMapControlChanging(CancelEventArgs cea)
        {
            var h = MapControlChanging;
            if (h != null) h(this, cea);
        }

        /// <summary>
        /// Event raised when the <see cref="MapControl"/> has changed.
        /// </summary>
        public event EventHandler MapControlChanged;

        /// <summary>
        /// The event invoker 
        /// </summary>
        /// <remarks>Override this method to wire to <see cref="MapBox"/>s events. Don't forget to call <c>base.OnMapControlChanged(e);</c> to invoke the event.</remarks>
        /// <param name="e">The event arguments</param>
        protected virtual void OnMapControlChanged(EventArgs e)
        {
            var h = MapControlChanged;
            if (h != null) h(this, e);
        }

        /// <summary>
        /// Gets or sets a value indicating the map control
        /// </summary>
        public SharpMap.Forms.MapBox MapControl
        {
            get { return _mapControl; }
            set
            {
                if (value == _mapControl)
                    return;

                var cea = new CancelEventArgs(false);
                OnMapControlChanging(cea);
                if (cea.Cancel) return;
                _mapControl = value;
                OnMapControlChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Event raised when <see cref="Enabled"/> changed.
        /// </summary>
        public event EventHandler EnabledChanged;

        /// <summary>
        /// Event invoker for the <see cref="EnabledChanged"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnEnabledChanged(EventArgs e)
        {
            var h = EnabledChanged;
            if (h != null) h(this, e);
        }

        /// <summary>
        /// Gets or sets a value indicating if the tool is enabled or not
        /// </summary>
        public bool Enabled
        {
            get { return _mapControl != null && _enabled; }
            set
            {
                if (value == _enabled)
                    return;
                _enabled = value;
                OnEnabledChanged(EventArgs.Empty);
            }
        }
    }
}
