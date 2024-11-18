#$appName = "BeStill"
#$apiKey = "67359f67ce666a410ea776958e79f5d1"

#$response = Invoke-WebRequest -Uri https://api.scripture.api.bible/v1/bibles -Method Get -Headers @{'Accept' = 'application/json'; 'api-key' = '67359f67ce666a410ea776958e79f5d1'}

#$convertedContent = ConvertFrom-Json $response.Content

#$kjvBibleId = "de4e12af7f28f599-01"

curl -X GET "https://api.scripture.api.bible/v1/bibles/de4e12af7f28f599-01/search?query=be%20still%20God&limit=100&sort=relevance" -H "accept: application/json" -H "api-key: 67359f67ce666a410ea776958e79f5d1"

$searchResponse = Invoke-WebRequest -Uri "https://api.scripture.api.bible/v1/bibles/de4e12af7f28f599-01/search?query=honest&limit=10&sort=relevance" -Method Get -Headers @{'Accept' = 'application/json'; 'api-key' = '67359f67ce666a410ea776958e79f5d1'}

$convertedSearchContent = ConvertFrom-Json $searchResponse.Content

$convertedSearchContent.data.verses | select reference,text | Export-Csv -Path C:\Users\dev\Desktop\LaunchPad\Assets\StreamingAssets\Data\Alternate\KJV_Honesty.csv -NoTypeInformation