﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gibson.Indexing;
using Gibson.Model;
using Sitecore.Diagnostics;

namespace Gibson.Storage
{
	public abstract class IndexedSerializationStore : ISerializationStore
	{
		private readonly IIndex _index;

		protected IndexedSerializationStore(IIndex index)
		{
			Assert.ArgumentNotNull(index, "index");

			_index = index;
		}

		/// <summary>
		/// Saves an item into the store
		/// </summary>
		public abstract void Save(ISerializableItem item);

		/// <summary>
		/// Loads an item from the store by ID
		/// </summary>
		/// <returns>The stored item, or null if it does not exist in the store</returns>
		public virtual ISerializableItem GetById(Guid itemId)
		{
			var itemById = _index.GetById(itemId);
			return Load(itemById, false);
		}

		public virtual IEnumerable<ISerializableItem> GetByPath(string path)
		{
			var itemsOnPath = _index.GetByPath(path);

			return itemsOnPath.Select(x => Load(x, true));
		}

		public virtual IEnumerable<ISerializableItem> GetByTemplate(Guid templateId)
		{
			var itemsOfTemplate = _index.GetByTemplate(templateId);

			return itemsOfTemplate.Select(x => Load(x, true));
		}

		public virtual IEnumerable<ISerializableItem> GetChildren(Guid parentId)
		{
			var childItems = _index.GetChildren(parentId);

			return childItems.Select(x => Load(x, true));
		}

		public virtual IEnumerable<ISerializableItem> GetDescendants(Guid parentId)
		{
			var descendants = _index.GetDescendants(parentId);

			return descendants.Select(x => Load(x, true));
		}

		/// <summary>
		/// Loads all items in the data store
		/// </summary>
		public abstract void CheckConsistency(bool fixErrors, Action<string> logMessageReceiver);

		/// <summary>
		/// Removes an item from the store
		/// </summary>
		/// <returns>True if the item existed in the store and was removed, false if it did not exist and the store is unchanged.</returns>
		public abstract bool Remove(Guid itemId);

		protected abstract ISerializableItem Load(IndexEntry indexData, bool assertExists);
	}
}