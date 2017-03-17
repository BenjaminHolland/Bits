# Bits
Bit Level Byte Array Windowing Library for C#
This libary contains classes that allow for easy handling of bit packed integers. It addresses many issues not addressed in other libraries. 

In particular: 
1. It allows sub-byte sized signed and unsigned integers (Even down to 1 bit signed. It's just 0 or -1).
2. It allows for arbitrarily large integers. 
3. It allows unaligned packing inside a byte array.
4. It allows arbitrary bit ordering.
	* Big & Little Byte/Bit ordering indexers are provided. 
	* Interface is provided for creating arbitrary bit orders. 
5. It allows easy transformation from one bit ordering to another.
6. It allows conversion to and from built in types.
	*. All Integer Types. 
	*. Character Type 
	*. String Type (In Testing) 
	*. Any Marshalable Struct (Planned) 
7. It provides an intuitive, feature-rich interface.

# Tour
## The BitWindow Class

The major workhorse of the library is the BitWindow. BitWindows are "windows" into a byte array. Each window stores a reference to 				this array (the *Source* array), and records information on when the window opens and closes. It then provides bit-level access to 				the bit values from the biginning to the end of the window.
	
In addition to storing the dimensions of the window, each BitWindow references an indexer that maps bit powers to indexes in the window. This allows reading and writing from differently formatted windows to be done without any goofy bit-twiddling visible to the caller.It also ensure that no matter what the underlying representation is, operations on the bit window follow a consistentpattern. For example, enumeration order of bits is the same regardless of how they're stored in the array.
