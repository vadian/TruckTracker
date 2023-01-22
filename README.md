# TruckTracker 
## A San Francisco Food Truck Locator

---

## Getting Started
### Docker image location:
https://hub.docker.com/repository/docker/carlmcodes/trucktracker/tags

This is a windows Docker container.  Please map ports as 80:80 and 443:443 and connect to `http://localhost:80/swagger/index.html` to access Swagger documentation and live queries.  Alternatively, this solution can be debugged with IIS Express or Docker in VS2022.  Tests are ran in the process of building the Docker image, but can also be ran manually in VS2022 or via `dotnet test`.

---
## Objectives:

* Provide search functionality of permitted food trucks within the San Francisco City jurisdiction by means of:
    * Applicant Name (Business Name)
    * Street Address
    * Geospatial Coordinates 
* Provide caching functions where applicable
* Provide live data where query functionality supported by OData Server
* Deploy via Dockerfile with Swagger

---
## Implementation:

The controller layer has been implemented using attribute-based routing.  This layer is kept minimal, and lambda methods are used where possible.  No logic happens in the controller layer.  

Two controllers are present.  One is responsible for handling live queries to the San Francisco OData Server, and the other is responsible for queries that use cached, local data.  Dependency injection was used to provide corresponding TruckTracker classes that manage the data to these controllers.

Only the cached TruckTracker has a (bonus) endpoint to fetch all data, and the GIS search function is also restricted to cached data.  The cache is updated when a query is received if the cache is more than 5 minutes out of date.

Unfortunately, the logic which handles the OData queries is not easily testable, as it is reliant on generated code.  All query logic that operates on cached data is under unit tests.  XUnit tests, with all setup restricted to the test following best practices, is utilized.

Windows Authorization was used, but for purposes of this project, Anonymous Authorization is explicitly allowed for these Controllers.

It was noted that a CSV of this data was provided via link with the problem description, and an intentional decision to use the live data provided was made for purposes of both data freshness and to allow the author to use an API he had not used before, OData.

With the decision to use ASP.NET Core, most architectural decisions flowed from current .NET Core best practices.  Non-Microsoft dependencies are only used for Swagger documentation and JSON parsing, and dependencies (both external and package) are kept to a minimum.

---

## Critique

### What would you have done differently if you had spent more time on this?
* I would have taken the time to investigate the rate of update of the Data to determine a more appropriate caching rate.  I would provide this as an injected variable, possibly by environment variables, for later configuration. 

* I would automate the build, test, and deployment process.

* As written, under heavy load, it is possible that multiple attempts to update the cache can happen.  This is sloppy, and could be reworked.

### What are the trade-offs you might have made?
* Caching is always a trade-off.  A 5 minute cache should be more than sufficient for this project, in fact, it is probably unnecessarily fast, wasting data.

* Personally, this author prefers functional approaches for data manipulation. This is impossible with the Microsoft OData Client, and OOP approaches were consistently used throughout this project.

* Test coverage is not the best.  It is over 50% coverage on the logical portion of the application, `TruckTracker.Data`.  However, it does not exercise all edge cases.

### What are the things you left out?
* Actual Authentication/Authorization
* Logging
* Class/method summaries
* Caching to local disk was initially implemented, but cannot be used with OData generated objects.  This dead code was left in, as it could be used if a decision is made to map the data to a slimmer class.  Dead code is a smell, so I'd address that sooner rather than later.

### What are the problems with your implementation and how would you solve them if we had to scale the application to a large number of users?
* Cache update logic - this does not cleanly handle multiple concurrent requests to update the cache.  This will not scale well, but can be resolved by adding a CacheUpdateProvider class which is used to make the cache update requests effectively idempotent.
* All data available via the OData API is currently provided to the calling user.  This is data hungry, and in production, this should be pruned back to the relevant data by use of a Response object.  This is also a security improvement in many production APIs, though that is not a concern here, as all data is already public.
* All live API endpoints should be disabled with a large number of users.  For this project, the data set itself is trivial and can be held entirely in memory.  This allows for the caching we've used here, and is a consideration of which we should take advantage.  These live endpoints are really just here to show I can easily handle either approach.
* Currently, this is a single docker microservice that handles its own cache and retrieves its own data.  For a more robust system, a shared volume might be used allowing for multiple instances of the service behind a load balancer to share a cache, scaling our API horizontally while maintaining the same number of requests to the upstream data source.  However, each instance maintaining its own cache is the most resilient approach.

### Time Spent: ~10-12 hours