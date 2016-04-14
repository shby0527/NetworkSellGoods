using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkSellFood
{
	public class WebIndentGoodsCollection:CollectionBase,IList<WebIndentGoods>,ILimitData
	{
		public WebIndentGoodsCollection ()
		{
		}

		public uint Page {
			get;
			set;
		}

		public uint PageCount {
			get;
			set;
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public int IndexOf (WebIndentGoods item)
		{
			return this.InnerList.IndexOf (item);
		}

		public void Insert (int index, WebIndentGoods item)
		{
			this.InnerList.Insert (index, item);
		}

		public void Add (WebIndentGoods item)
		{
			this.InnerList.Add (item);
		}

		public bool Contains (WebIndentGoods item)
		{
			return this.InnerList.Contains (item);
		}

		public void CopyTo (WebIndentGoods[] array, int arrayIndex)
		{
			this.InnerList.CopyTo (array, arrayIndex);
		}

		public bool Remove (WebIndentGoods item)
		{
			this.InnerList.Remove (item);
			return true;
		}

		IEnumerator<WebIndentGoods> IEnumerable<WebIndentGoods>.GetEnumerator ()
		{
			foreach (object i in this.InnerList) {
				yield return i as WebIndentGoods;
			}
		}

		public WebIndentGoods this [int index] {
			get {
				return this.InnerList [index] as WebIndentGoods;
			}
			set {
				this.InnerList [index] = value;
			}
		}
	}
}

