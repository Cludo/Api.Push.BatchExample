# BatchExample
Batch push is way how customer can push data our api. There is two reasons to use batch:
* Large amount of pages rarely changed
* Search pages/documents are in internal network where crawler cant get access
 
#How to use
Batch api is pretty simple app should make following request

    POST http://api.cludo.com/api/v3/{CustomerID}/content/{ContentId}/push
    Authorization: Basic Base64(CustomerId:CustomerKey)
    [{"Id": "UNIQUE&REQUIRED", "Type": "PageContent|MemberInfo|FileContent&REQUIRED", "Title": "", "OtherFields":["Could", "Be", "array"]}]
    
# C# example
[Example how to push](https://github.com/Cludo/BatchExample/blob/master/BatchApiExample/BatchApiExample/Program.cs)
