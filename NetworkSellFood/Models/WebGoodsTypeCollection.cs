using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkSellFood
{
	public class WebGoodsTypeCollection:CollectionBase,IList<WebGoodsTypes>
	{
		public WebGoodsTypeCollection ()
		{
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public int IndexOf (WebGoodsTypes item)
		{
			return this.InnerList.IndexOf (item);
		}

		public void Insert (int index, WebGoodsTypes item)
		{
			this.InnerList.Insert (index, item);
		}

		public void Add (WebGoodsTypes item)
		{
			this.InnerList.Add (item);
		}

		public bool Contains (WebGoodsTypes item)
		{
			return this.InnerList.Contains (item);
		}

		public void CopyTo (WebGoodsTypes[] array, int arrayIndex)
		{
			this.InnerList.CopyTo (array, arrayIndex);
		}

		public bool Remove (WebGoodsTypes item)
		{
			this.InnerList.Remove (item);
			return true;
		}

		IEnumerator<WebGoodsTypes> IEnumerable<WebGoodsTypes>.GetEnumerator ()
		{
			foreach (object i in this.InnerList) {
				yield return i as WebGoodsTypes;
			}
		}

		public WebGoodsTypes this [int index] {
			get {
				return this.InnerList [index] as WebGoodsTypes;
			}
			set {
				this.InnerList [index] = value;
			}
		}
	}
}

