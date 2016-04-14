using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkSellFood
{
	public class WebGoodsCollection:CollectionBase,IList<WebGoodsInfo>,ILimitData
	{
		public WebGoodsCollection ()
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

		public int IndexOf (WebGoodsInfo item)
		{
			return this.InnerList.IndexOf (item);
		}

		public void Insert (int index, WebGoodsInfo item)
		{
			this.InnerList.Insert (index, item);
		}

		public void Add (WebGoodsInfo item)
		{
			this.InnerList.Add (item);
		}

		public bool Contains (WebGoodsInfo item)
		{
			return this.InnerList.Contains (item);
		}

		public void CopyTo (WebGoodsInfo[] array, int arrayIndex)
		{
			this.InnerList.CopyTo (array, arrayIndex);
		}

		public bool Remove (WebGoodsInfo item)
		{
			this.InnerList.Remove (item);
			return true;
		}

		IEnumerator<WebGoodsInfo> IEnumerable<WebGoodsInfo>.GetEnumerator ()
		{
			foreach (object i in this.InnerList) {
				yield return i as WebGoodsInfo;
			}
		}

		public WebGoodsInfo this [int index] {
			get {
				return this.InnerList [index] as WebGoodsInfo;
			}
			set {
				this.InnerList [index] = value;
			}
		}
	}
}

