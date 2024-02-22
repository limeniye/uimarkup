using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Extended;

public static class NotifyDictionaryChangedEventArgsExtensions
{
	/// <summary>
	/// Checks for missing elements, the presence of new elements, or a change of value based on another dictionary
	/// </summary>
	public static void NewElementsHandler<TKey, TValue>(
		this IDictionary<TKey, TValue> currentEntities,
		object sender,
		IDictionary<TKey, TValue> newEntities,
		EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> action)
	{
		lock (((ICollection)currentEntities).SyncRoot)
		{
			if (newEntities.Count <= 0)
			{
				currentEntities.Clear();
				action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ClearKeyValuePairs<TKey, TValue>());
				return;
			}

			//remove old items and change new items
			for (int i = currentEntities.Count - 1; i >= 0; i--)
			{
				var index = i;
				var entityKey = currentEntities.Keys.ElementAt(index);
				if (newEntities.TryGetValue(entityKey, out var newEntity))
				{
					var oldEntity = currentEntities[entityKey];
					if (!Equals(oldEntity, newEntity))
					{
						currentEntities[entityKey] = newEntity;
						action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ChangeKeyValuePair(entityKey, oldEntity, newEntity));
					}
					newEntities.Remove(entityKey);
				}
				else
				{
					currentEntities.Remove(entityKey);
					action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<TKey, TValue>(entityKey));
				}
			}

			//add new items
			if (newEntities.Count > 0)
			{
				foreach (var newItemPair in newEntities)
				{
					currentEntities.Add(newItemPair.Key, newItemPair.Value);
					action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(newItemPair.Key, newItemPair.Value));
				}
			}
		}
	}

	/// <summary>Adding a new key-value pair to the dictionary</summary>
	/// <remarks>
	/// Returns false if such a key already exists and the addition has not been performed
	/// </remarks>
	public static bool AddAndRiseEvent<TKey, TValue>(
		this IDictionary<TKey, TValue> currentEntities,
		object sender,
		EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> action,
		TKey addKey, TValue addValue)
	{
		if (!currentEntities.TryAdd(addKey, addValue))
		{
			return false;
		}
		action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(addKey, addValue));
		return true;
	}

#if NETSTANDARD2_0
	public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
	{
		if (dictionary == null)
		{
			throw new ArgumentNullException(nameof(dictionary));
		}

		if (!dictionary.ContainsKey(key))
		{
			dictionary.Add(key, value);
			return true;
		}
		return false;
	}
#endif

	/// <summary>
	/// Removing a key-value pair from the dictionary
	/// </summary>
	/// <returns>
	/// Returns false if no such key exists and no deletion was performed
	/// </returns>
	public static bool RemoveAndRiseEvent<TKey, TValue>(
		this IDictionary<TKey, TValue> currentEntities,
		object sender,
		EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> action,
		TKey removeKey)
	{
		if (currentEntities.Remove(removeKey))
		{
			action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<TKey, TValue>(removeKey));
			return true;
		}
		return false;
	}

	/// <summary>
	/// Removing a key-value pair from the dictionary
	/// </summary>
	/// <returns>
	/// Returns false if no such key exists and no deletion was performed
	/// </returns>
	public static bool RemoveAndRiseEvent<TKey, TValue>(
		this IDictionary<TKey, TValue> currentEntities,
		object sender,
		EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> action,
		TKey removeKey,
		TValue oldValue)
	{
		if (currentEntities.Remove(removeKey))
		{
			action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair(removeKey, oldValue));
			return true;
		}
		return false;
	}

	/// <summary>
	/// Changing the key value in the dictionary
	/// </summary>
	public static void ChangeAndRiseEvent<TKey, TValue>(
		this IDictionary<TKey, TValue> currentEntities,
		object sender,
		EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> action,
		TKey changeKey,
		TValue changeValue)
	{
		if (currentEntities.TryGetValue(changeKey, out TValue oldValue))
		{
			currentEntities[changeKey] = changeValue;
			action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ChangeKeyValuePair(changeKey, oldValue, changeValue));
		}
		else
		{
			throw new InvalidOperationException($"The key {changeKey} doesn't exists in {currentEntities} dictionary");
		}
	}

	/// <summary>
	/// Setting the key value in the dictionary
	/// </summary>
	/// <returns>
	/// Returns false if there is no such key and instead of replacement, addition was performed
	/// </returns>
	public static bool SetAndRiseEvent<TKey, TValue>(
		this IDictionary<TKey, TValue> currentEntities,
		object sender,
		EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> action,
		TKey changeKey,
		TValue changeValue)
	{
		if (currentEntities.TryGetValue(changeKey, out TValue oldValue))
		{
			currentEntities[changeKey] = changeValue;
			action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ChangeKeyValuePair(changeKey, oldValue, changeValue));
			return true;
		}

		currentEntities.Add(changeKey, changeValue);
		action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(changeKey, changeValue));
		return false;
	}

	/// <summary>
	/// Setting the key value in the dictionary
	/// </summary>
	/// <returns><see cref="NotifyDictionaryChangedAction.Added"/> if value was added<br/>
	/// <see cref="NotifyDictionaryChangedAction.Changed"/> if item was changed<br/>
	/// <see cref="Nullable"/> result if nothing was happened</returns>
	/// <remarks> <paramref name="changeValue"/> cannot be null </remarks>
	public static NotifyDictionaryChangedAction? EqualBeforeAddOrSetAndRiseEvent<TKey, TValue>(
		this IDictionary<TKey, TValue> currentEntities,
		object sender,
		EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> action,
		TKey changeKey,
		TValue changeValue)
	{
		if (currentEntities.TryGetValue(changeKey, out TValue oldValue))
		{
			if (oldValue == null || !oldValue.Equals(changeValue))
			{
				currentEntities[changeKey] = changeValue;
				action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ChangeKeyValuePair(changeKey, oldValue, changeValue));
				return NotifyDictionaryChangedAction.Changed;
			}
			return null;
		}

		currentEntities.Add(changeKey, changeValue);
		action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(changeKey, changeValue));
		return NotifyDictionaryChangedAction.Added;
	}

	/// <summary>
	/// Clearing the dictionary
	/// </summary>
	/// <returns>
	/// Returns false if the dictionary was empty
	/// </returns>
	public static bool ClearAndRiseEvent<TKey, TValue>(
		this IDictionary<TKey, TValue> currentEntities,
		object sender,
		EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> action)
	{
		var isEmpty = currentEntities.Count != 0;

		if (isEmpty)
		{
			currentEntities.Clear();
			action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ClearKeyValuePairs<TKey, TValue>());
			return true;
		}
		return false;
	}

	public static bool ClearAndRiseEvent<TKey, TValue>(
		this IDictionary<TKey, TValue> currentEntities,
		object sender,
		EventHandler<NotifyDictionaryChangedEventArgs<TKey, TValue>> action,
		IDictionary<TKey, TValue> oldItems)
	{
		var isEmpty = currentEntities.Count != 0;

		if (isEmpty)
		{
			currentEntities.Clear();
			action?.Invoke(sender, NotifyActionDictionaryChangedEventArgs.ClearKeyValuePairs(oldItems));
			return true;
		}
		return false;
	}
}
