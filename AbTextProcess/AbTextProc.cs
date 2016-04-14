using System;
using PluginLoader.Plugins;

namespace AbTextProcess
{
	public abstract class AbTextProc:IPlugin
	{
		protected AbTextProc ()
		{
		}
		#region IPlugin implementation
		/// <summary>
		/// Loading this instance.
		/// </summary>
		public virtual bool Loading ()
		{
			this.InputText = "";
			return true;
		}

		/// <summary>
		/// Unloading this instance.
		/// </summary>
		/// <returns><c>true</c>, if loading was uned, <c>false</c> otherwise.</returns>
		public virtual bool UnLoading ()
		{
			return true;
		}
		#endregion
		/// <summary>
		/// Gets or sets the inputed text.
		/// </summary>
		/// <value>The input text.</value>
		public string InputText{ get; set; }

		/// <summary>
		/// Gets the processed text.
		/// </summary>
		/// <value>The processed text.</value>
		public string ProcessedText {

			get {
				if (this.InputText == "")
					return "";
				return this.Processing ();
			}
		}

		/// <summary>
		/// Processing this Text.
		/// </summary>
		protected abstract string Processing ();

		/// <summary>
		/// Gets or sets a value indicating when you well to end the argument to transfer.
		/// </summary>
		/// <value><c>true</c> if is end trans; otherwise, <c>false</c>.</value>
		public bool isEndTrans{ get; set; }
	}
}

