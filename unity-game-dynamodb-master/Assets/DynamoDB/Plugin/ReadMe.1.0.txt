//----------------------------------------------
// DynamoDB Helper
// Copyright ? 2015 OuijaPaw Games LLC
//----------------------------------------------

------------------------------
Changelog
------------------------------
v 1.0.0:
- Initial release of DynamoDBHTTP



------------------------------
Important Information
------------------------------
1.  No prerequisites.  This uses HTTP calls to connect to DynamoDB.  .NET Sockets / etc. is not used.
2.  This is information on how to connect, not how to write the JSON strings.  Please consult
    DynamoDB documentation for writing different JSON queries / deletes / updates / etc.


------------------------------
Setup
------------------------------
1.  Create Amazon Web Services (AWS) account
2.  Create a DynamoDB table
    - For testing this package, create a table with these properties (Case Matters!):
	    - Name = DDBHelper
		- HashKey = Name (data type String)
		- RangeKey = Type (data type String)
    - Note that the free-tier DynamoDB limits are 25 read/second and 25 write/sec
	- Note that indexes can only be created when table is created
3.  Record access key and secret key somewhere
4.  Import DynamoDBHTTP package.  It's just one script really...
5.  Create an empty GameObject in the scene
6.  Add the DBUnityHelper script object to the object, fill out AwS Key and Secret Key
10. Run scene.  Console should show you information about

Go about examining the code how this accomplished.


