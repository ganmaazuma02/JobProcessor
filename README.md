# Integer Array Sorting Background Job Processor

This background job processor handles the processing of jobs in a queue (FIFO). 
Whenever a user triggers the POST API method to enqueue a job, this will then save a job record to the queue with a job status of "Queued".
This solution runs on .NET 6 LTS.

Each job consists of these columns:
1. JobId - Guid
2. JobEnqueuedDateTimeUtc - DateTime (The DateTime at UTC when the job was queued)
3. JobProcessingDurationMiliseconds - Long integer (The job processing duration from start to finish in miliseconds)
4. JobStatus - Integer (The job statuses: 0 - "Queued", 1 - "Processing", 2 - "Completed"
5. JobInput - String (The job input which is a comma separated integers in string format)
6. JobOutput - String (The job output which is a comma separated integers in string format)

The processor will monitor the queue to see which job was queued the earliest and then starts the processing.
The process will first update the job status to "Processing" and while it is processing it will not look for any other job.
Until the process is finished, the job status will then be updated to "Completed" and proceeds with monitoring for a new job.
This is useful if the job is a long processing CPU-intensive job.

You can configure the job checking frequency by changing the config "JobCheckingFrequencyInSeconds" in the appsettings.json file. 
There is also a configuration called "IsFakeLongRunningTask" which is just for the sake of the take home exercise. This setting will mimic the sorting job as if it is a long running job (7 seconds).
![image](https://user-images.githubusercontent.com/55015047/182481436-8acc6b05-958c-4c3f-b151-df8a6b1e6aa4.png)


## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. 

### Running the solution on localhost
This solution is built using Dockerfile and thus you will need to install Docker Desktop in order to build and run it.
The easiest way to run on localhost is by using Visual Studio 2022. You will get a prompted to install all the necessary softwares before you are able to build, run, and test it.

1. Clone this repo
2. Install .NET 6
3. Install Visual Studio 2022
4. Install Docker Desktop
5. Install WSL2 for Windows
6. Open the solution "JobProcessor.sln" in Visual Studio 2022
7. Run the project in Visual Studio 2022 by selecting Docker as the running engine.
![image](https://user-images.githubusercontent.com/55015047/182480941-2bf21336-8ff9-4c4a-aff5-96f72f7737fb.png)

There will be loggings such as these to know what exactly the background job is doing.
![image](https://user-images.githubusercontent.com/55015047/182482299-b9808593-0a03-451a-95c9-598b330c7f5e.png)
![image](https://user-images.githubusercontent.com/55015047/182482363-5a3a0f8a-e1af-4186-8b41-88d6cd2de002.png)


### Unit test
You can run the unit test by opening the solution in Visual Studio 2022 and click on the tab Test > Run All Tests.

### Manual Testing
Once you run the solution locally, it will automatically brings you to the Swagger OpenAPI page. You can test the API endpoints there.
There are 3 endpoints:
1. Get all jobs.
2. Create a job by sending the array of integers as the parameter.
3. Get a job by ID.

![image](https://user-images.githubusercontent.com/55015047/182481738-eb97f17b-581c-4de8-bec1-4ca39cdea17e.png)
