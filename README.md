# BatchExample
Cludo supports the push of data to Cludo. By pushing data to Cludo you will get an instant update of your search index.
If you have any questions to the API, don’t hesitate to contact us at support@cludo.com 

 
#How to use the API
To push data, you only need to know about two methods, one for inserting and updating content and another for deleting existing content.

For both API requests you need to supply your CustomerID and the ContentID. You also need to supply a Base64 encrypted version of your CustomerKey as authorization header. 

#How to add data:
To add data to Cludo simply use:
> https://api.cludo.com/api/v3/{CustomerID}/content/{ContentID}/push

The push works as an upsert, so if you try to push data with an ID which already exists it will just update it, if it doesn’t exist it will just insert it as a new record.
You can push an array of items to Cludo, which makes it easy to push a batch of updated pages or files. The data you push to Cludo should be in the following format: 
```
  [{
	  Id: // Add unique id,
	  Type: // Which content type are you pushing, you can choose between PageContent, MemberInfo and FileContent. 
  }]
  ```
*Note*: Id and type are the only two required fields.

Besides the two fields you can push the data you want in the index. You just supply the name and the object – you can even push an array. An example could be:
```
[{	Id: 1,
	Type: “PageContent”,
	Title: “Brown shoes”,
	Sizes: [8,	9,	10],
 instock: "yes"},
{	Id: 2,
	Type: "PageContent",
	Title: “Black shoes”,
	Sizes: [7, 8],
 instock: "no",
 available: "2016-05-10"
}]
```
#cUrl example of push Data
```
curl -i -H "Authorization: Basic Base64(CUSOMTERID:CUSTOMERKEY)" -H "Content-Type: application/json" --data "[{Id: 1, Type: \"PageContent\",    Title: \"Brown shoes\",    Sizes: [8,  9,  10], instock: \"yes\"},{Id: 2, Type: \"PageContent\", Title: \"Black shoes\", Sizes: [7, 8], instock: \"no\", available: \"2016-05-10\"}]" https://api.cludo.com/api/v3/{CUSTOMERID}/content/{CONTENTID}/push 
```
#How to delete data
To delete an item from the index use: 
> https://api.cludo.com/api/v3/{CustomerID}/content/{ContentId}/delete

The data you supply is an array of key value pairs like the following:
```
[{	
 "your unique id": "Type" // Which content type are you deleting, you can choose between PageContent, MemberInfo and FileContent.
}]
```
An example is 
```
[{"1": "PageContent"},
{"2", "PageContent"}]
```

#cUrl example 
```
curl -i -H "Authorization: Basic Base64(CUSOMTERID:CUSTOMERKEY)" -H "Content-Type: application/json" --data "[{\"1\": \"PageContent\"}, {\"2\": \"PageContent\"}]" https://api.cludo.com/api/v3/{CUSTOMERID}/content/{CONTENTID}/delete
```

# C# example
[Example how to push](https://github.com/Cludo/BatchExample/blob/master/BatchApiExample/BatchApiExample/Program.cs)
