using System.Collections.Generic;

namespace System.Extended.EventArgs;

public enum NotifyDictionaryChangedAction
{
	Added,
	Removed,
	Changed,
	Cleared,
	Initialized
}

public class NotifyDictionaryChangedEventArgs<TKey, TValue> : NotifyActionDictionaryChangedEventArgs
{
	public TKey Key { get; }

	/// <summary>Deleted or modified value</summary>
	public TValue OldValue { get; }

	public TValue NewValue { get; }

	public IDictionary<TKey, TValue> NewDictionary { get; }

	public IDictionary<TKey, TValue> OldDictionary { get; }

	/// <summary>
	/// Constructor for creating an add, delete, or change action
	/// </summary>
	public NotifyDictionaryChangedEventArgs(
		NotifyDictionaryChangedAction action,
		TKey key,
		TValue oldValue,
		TValue newValue)
		: base(action)
	{
		Key = key;
		OldValue = oldValue;
		NewValue = newValue;
	}

	/// <summary>
	/// A constructor for creating an initialization or cleanup action
	/// </summary>
	public NotifyDictionaryChangedEventArgs(
		NotifyDictionaryChangedAction action,
		IDictionary<TKey, TValue> newDictionary,
		IDictionary<TKey, TValue> oldDictionary)
		: base(action)
	{
		NewDictionary = newDictionary;
		OldDictionary = oldDictionary;
	}
}

public class NotifyActionDictionaryChangedEventArgs : System.EventArgs
{
	/// <summary>The action changed the dictionary</summary>
	public NotifyDictionaryChangedAction Action { get; }

	/// <summary>Creates an instance</summary>
	/// <param name="action">The action changed the dictionary</param>
	public NotifyActionDictionaryChangedEventArgs(NotifyDictionaryChangedAction action)
	{
		Action = action;
	}

	/// <summary>
	/// Creating an argument for the notification event about adding a new key-value pair to the dictionary
	/// </summary>
	public static NotifyDictionaryChangedEventArgs<TKey, TValue> AddKeyValuePair<TKey, TValue>(TKey key, TValue value)
	  => new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Added, key, default, value);

	/// <summary>
	/// Creating an argument for the notification event about removing a key-value pair from the dictionary
	/// </summary>
	public static NotifyDictionaryChangedEventArgs<TKey, TValue> RemoveKeyValuePair<TKey, TValue>(TKey key, TValue oldValue = default)
		=> new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Removed, key, oldValue, default);

	// Creation of an argument for the notification event about the replacement of the key value in the dictionary
	public static NotifyDictionaryChangedEventArgs<TKey, TValue> ChangeKeyValuePair<TKey, TValue>(TKey key, TValue oldValue, TValue newValue)
		=> new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Changed, key, oldValue, newValue);

	// Creating an argument for the notification event about the complete replacement of all dictionary elements
	public static NotifyDictionaryChangedEventArgs<TKey, TValue> InitializeKeyValuePairs<TKey, TValue>(IDictionary<TKey, TValue> values)
		=> new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Initialized, values, default);

	// Creation of an argument for the notification event about clearing the dictionary (that is, removing all key-value pairs)
	public static NotifyDictionaryChangedEventArgs<TKey, TValue> ClearKeyValuePairs<TKey, TValue>()
		=> new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Cleared, default, default, default);

	public static NotifyDictionaryChangedEventArgs<TKey, TValue> ClearKeyValuePairs<TKey, TValue>(IDictionary<TKey, TValue> oldValues)
		=> new NotifyDictionaryChangedEventArgs<TKey, TValue>(NotifyDictionaryChangedAction.Cleared, default, oldValues);
}
