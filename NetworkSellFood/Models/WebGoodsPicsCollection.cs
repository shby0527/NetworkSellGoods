using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkSellFood
{
	public class WebGoodsPicsCollection:CollectionBase,IList<WebGoodsPic>
	{
		public WebGoodsPicsCollection ()
		{
		}

		public int IndexOf (WebGoodsPic item)
		{
			return this.InnerList.IndexOf (item);
		}

		public void Insert (int index, WebGoodsPic item)
		{
			this.InnerList.Insert (index, item);
		}

		public void Add (WebGoodsPic item)
		{
			this.InnerList.Add (item);
		}

		public bool Contains (WebGoodsPic item)
		{
			return this.InnerList.Contains (item);
		}

		public void CopyTo (WebGoodsPic[] array, int arrayIndex)
		{
			this.InnerList.CopyTo (array, arrayIndex);
		}

		public bool Remove (WebGoodsPic item)
		{
			this.InnerList.Remove (item);
			return true;
		}

		IEnumerator<WebGoodsPic> IEnumerable<WebGoodsPic>.GetEnumerator ()
		{
			foreach (object i in this.InnerList) {
				yield return i as WebGoodsPic;
			}
		}

		public WebGoodsPic this [int index] {
			get {
				return this.InnerList [index] as WebGoodsPic;
			}
			set {
				this.InnerList [index] = value;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}
	}
}

