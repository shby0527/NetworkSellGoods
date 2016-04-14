using System;
using System.Collections;
using System.Collections.Generic;

namespace NetworkSellFood
{
	public class WebForGoodsReplayCollection:CollectionBase,IList<WebForGoodsReplay>
	{
		public WebForGoodsReplayCollection ()
		{
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public int IndexOf (WebForGoodsReplay item)
		{
			return this.InnerList.IndexOf (item);
		}

		public void Insert (int index, WebForGoodsReplay item)
		{
			this.InnerList.Insert (index, item);
		}

		public void Add (WebForGoodsReplay item)
		{
			this.InnerList.Add (item);
		}

		public bool Contains (WebForGoodsReplay item)
		{
			return this.InnerList.Contains (item);
		}

		public void CopyTo (WebForGoodsReplay[] array, int arrayIndex)
		{
			this.InnerList.CopyTo (array, arrayIndex);
		}

		public bool Remove (WebForGoodsReplay item)
		{
			this.InnerList.Remove (item);
			return true;
		}

		IEnumerator<WebForGoodsReplay> IEnumerable<WebForGoodsReplay>.GetEnumerator ()
		{
			foreach (object i in this.InnerList) {
				yield return i as WebForGoodsReplay;
			}
		}

		public WebForGoodsReplay this [int index] {
			get {
				return this.InnerList [index] as WebForGoodsReplay;
			}
			set {
				this.InnerList [index] = value;
			}
		}
	}
}

