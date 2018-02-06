# unity-game-dynamodb
A simple Unity platformer game with AWS DyanmoDB logging capabilities

###Tutorial:
The logging system consists principally of three scripts: **Logger.cs**, **Dynode.cs**, and **DDBHTTP.cs**. A fourth script is used for the replaying of a logged path, allowing you to re-simulate exactly what the player did in a given run; this "replay" script is called **ScanDynamo.cs**.

---

#### Logging System:

The logging system is relatively straightforward.

1. First, **Logger.cs** should be attached to some object in the level scene (for example, the player). **Logger.cs** handles all input to be logged (*key strokes, x/y positions, etc.*), as well as logging frequency (*every 0.5 seconds, whenever a key is pressed, etc.*). In the Unity Inspector, you'll need to define a few public variables: your `AWS access key`, your `AWS access secret`, your DynamoDB table name, your table primary key, and optionally, a short debug string (named `Log` ). To toggle logging on and off, you can check/uncheck the `Logging` boolean.
2. Upon scene start, **Logger.cs** will create a *DontDestroyOnLoad* instance of **Dynode.cs**. This **Dynode.cs** instance is meant to persist between scene changes in Unity (hence, *DontDestroyOnLoad*), for the purpose of tracking run count in a given session. Besides the information to be logged (which is handled by **Logger.cs**), **Dynode.cs** handles all other attributes of each item that is put into the DynamoDB table; these include `run_count`, `action_count`, `session_id`, `run_id`, and the time stamp.
3. **Dynode.cs** then creates a *DontDestroyOnLoad* instance of **DDBHTTP.cs**, which is the script that handles the lowest-level http request building. This bulk of this script is courtesy of ***OuijaPaw Games LLC*** for free from the [Unity Asset Store](https://www.assetstore.unity3d.com/en/#!/content/21215). **DDBHTTP.cs** builds behind-the-scenes the http request, which includes headers, the JSON item, and a hashed signature.

---

#### Replaying logged runs
I threw together a quick script, **ScanDynamo.cs**, which allows you to replay runs by providing a valid `run_id` from your DynamoDB table. For setup, just attach the script to an object (in my case, I used a UI button so I could press it to replay the run), and have the Scan action called. Make sure you provide the script with the player object, and also your `AWS access key` and `AWS access secret`.

Depending on how you've logged your data, this script may change heavily; in my case, I logged key strokes (I had to use Windows Input Simulator. This is a quick-fix, but it works pretty well on my machine). Ultimately, however, **ScanDynamo.cs** uses recursive Coroutines determine when to execute the next logged event, and for how long, to re-simulate as closely as possible the player run. Inaccuracy may be due to lag, as each coroutine has its own WaitForSeconds timer.

> ***IMPORTANT NOTE***: When you're replaying a run using **ScanDynamo.cs**, make sure that the `Logging` public boolean on **Logger.cs** is FALSE. Otherwise, replaying a run will create duplicate data in the DynamoDB table, because it will be logging the replay of the run. This is admittedly a stupid flaw; don't worry, I'm getting to it.
