using System;
using System.Collections;

namespace XMLDB3
{
	public class LinkedHybridCache
	{
		private class LinkHeader : ISection, ICacheItem
		{
			private object context;

			private int count;

			public LinkItem first;

			public object Context
			{
				get
				{
					return context;
				}
				set
				{
					context = value;
				}
			}

			public int Count => count;

			public ILinkItem First => first;

			public LinkHeader(object _context)
			{
				count = 0;
				first = null;
				context = _context;
			}

			public void InsertItem(LinkItem _item)
			{
				_item.header = this;
				if (first == null)
				{
					first = _item;
					_item.next = null;
					_item.prev = null;
				}
				else
				{
					first.prev = _item;
					_item.prev = null;
					_item.next = first;
					first = _item;
				}
				count++;
			}

			public void RemoveItem(LinkItem _item)
			{
				if (first == _item)
				{
					first = _item.next;
					if (first != null)
					{
						first.prev = null;
					}
				}
				else
				{
					_item.prev.next = _item.next;
					if (_item.next != null)
					{
						_item.next.prev = _item.prev;
					}
				}
				_item.next = null;
				_item.prev = null;
				_item.header = null;
				count--;
			}

			public object[] ToArray()
			{
				if (count > 0)
				{
					object[] array = new object[count];
					LinkItem next = first;
					for (int i = 0; i < count; i++)
					{
						if (next == null)
						{
							break;
						}
						array[i] = next.Context;
						next = next.next;
					}
					return array;
				}
				return null;
			}

			public Array ToArray(Type type)
			{
				if (count > 0)
				{
					Array array = Array.CreateInstance(type, count);
					LinkItem next = first;
					for (int i = 0; i < count; i++)
					{
						if (next == null)
						{
							break;
						}
						array.SetValue(next.Context, i);
						next = next.next;
					}
					return array;
				}
				return null;
			}
		}

		private class LinkItem : ILinkItem, ICacheItem
		{
			public LinkItem next;

			public LinkItem prev;

			public LinkHeader header;

			private object context;

			public object Context
			{
				get
				{
					return context;
				}
				set
				{
					context = value;
				}
			}

			public ILinkItem Next => next;

			public ILinkItem Prev => prev;

			public ISection Section => header;

			public LinkItem(object _context)
			{
				context = _context;
				next = null;
			}
		}

		private Hashtable section;

		private Hashtable dictionary;

		public LinkedHybridCache()
		{
			section = new Hashtable();
			dictionary = new Hashtable();
		}

		public LinkedHybridCache(int _sectionSize, int _itemSize)
		{
			if (_sectionSize != 0)
			{
				section = new Hashtable(_sectionSize);
			}
			else
			{
				section = new Hashtable();
			}
			if (_itemSize != 0)
			{
				dictionary = new Hashtable(_itemSize);
			}
			else
			{
				dictionary = new Hashtable();
			}
		}

		public ILinkItem FindItem(object _key)
		{
			return (ILinkItem)dictionary[_key];
		}

		public ISection FindSection(object _section)
		{
			return (ISection)section[_section];
		}

		public ISection AddSection(object _section, object _data)
		{
			LinkHeader linkHeader = new LinkHeader(_data);
			section.Add(_section, linkHeader);
			return linkHeader;
		}

		public void RemoveSection(object _section, IKeyFinder _keyFinder)
		{
			LinkHeader linkHeader = (LinkHeader)section[_section];
			LinkItem first = linkHeader.first;
			for (int i = 0; i < linkHeader.Count; i++)
			{
				if (first == null)
				{
					break;
				}
				object key = _keyFinder.GetKey(first.Context);
				dictionary.Remove(key);
			}
			section.Remove(_section);
		}

		public ICacheItem AddItem(object _section, object _key, object _value)
		{
			LinkHeader linkHeader = (LinkHeader)section[_section];
			if (linkHeader == null)
			{
				throw new Exception(_section.ToString() + "에 해당하는 섹션이 없습니다.");
			}
			LinkItem linkItem = new LinkItem(_value);
			dictionary.Add(_key, linkItem);
			linkHeader.InsertItem(linkItem);
			return linkItem;
		}

		public void MoveSection(object _section, ILinkItem _item)
		{
			LinkItem linkItem = (LinkItem)_item;
			LinkHeader linkHeader = (LinkHeader)section[_section];
			if (linkHeader == null)
			{
				throw new Exception(_section.ToString() + "에 해당하는 섹션이 없습니다.");
			}
			linkItem.header.RemoveItem(linkItem);
			linkHeader.InsertItem(linkItem);
		}

		public void RemoveItem(object _key)
		{
			LinkItem linkItem = (LinkItem)dictionary[_key];
			if (linkItem != null)
			{
				linkItem.header.RemoveItem(linkItem);
				dictionary.Remove(_key);
			}
		}

		public ICollection GetSection()
		{
			return section.Values;
		}

		public Hashtable GetItems()
		{
			return (Hashtable)dictionary.Clone();
		}
	}
}
