# Santander Global Tech - Senior .NET Core Developer Test #3
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3&metric=bugs)](https://sonarcloud.io/dashboard?id=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3&metric=code_smells)](https://sonarcloud.io/dashboard?id=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3&metric=coverage)](https://sonarcloud.io/dashboard?id=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3&metric=sqale_index)](https://sonarcloud.io/dashboard?id=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=jpmoura_SantanderGlobalTech_NetCoreSenior_Test3)
[![CD](https://github.com/jpmoura/santander-global-tech-sr-net-core-dev-test-3/actions/workflows/cd.yml/badge.svg)](https://github.com/jpmoura/santander-global-tech-sr-net-core-dev-test-3/actions/workflows/cd.yml)
[![CI](https://github.com/jpmoura/santander-global-tech-sr-net-core-dev-test-3/actions/workflows/ci.yml/badge.svg)](https://github.com/jpmoura/santander-global-tech-sr-net-core-dev-test-3/actions/workflows/ci.yml)

This repo contains my implementation of the test #3 of [Santander Global Tech](https://santanderglobaltech.com/en/).

The goal is to build an API to fetch and transform data from [Hacker News API](https://github.com/HackerNews/API).

## 1. Instructions

To build, test and run this API you can use the `dotnet` console tool, shipped with .NET SDK. The commands fo those are:

1. **Build**: `dotnet restore && dotnet build`
2. **Test**: `dotnet test`
3. **Run**: `dotnet run --project SantanderGlobalTech.HackerNews.Api\SantanderGlobalTech.HackerNews.Api.csproj`

⚠ **Attention**: All the commands above have as premisse that your current working directory is the project root.

You could also use the [Dockerfile](SantanderGlobalTech.HackerNews.Api/Dockerfile) to build a container and use it as an instance for your test.

If you are familiar with [Cake](https://cakebuild.net/) build automation system you could also use it to build, test and run the app, but pay attention on [cake setup](#3-cake-setup-) steps.

## 2. Assumptions

1. In the future could exist the need to filter more than twenty best stories, so the use case was built to be flexible with this fetch limit.

2. It is possible that the user want to sort the stories by other prop than the score. Because of this, the order stories use case was built already with the possibility of use other props besides Score. The method responsible to get key selector for ordination process is the `OrderStoriesUseCase@GetKeySelectorFunc` and, right now, there is no logic in there to select other prop but a naive approach will be a creation of an enumeration of sortable `Story` props and use this enum as parameter in the use cases request.

3. This API could be used by a Frontend application, therefore, the inclusion of a OpenAPI definition and a documentation page (a.k.a. Swagger) that could be reached at `/swagger` endpoint.

4. Memory cache was used as cache mechanism to avoid query the HackerNews API every time. Distributed cache was not used due the time restriction to build this test project, but, with enough time my approach would be using Redis as a distributed cache. As it is, if more than one instance of this API is used, then every instance will have it own memory cache instance. The cache TTL is configurable via `appsetings.json` and its default is 5 minutes, so the response provide by this API could be not up to date with real time values.

5. There is a health check url at `/health` to help check if the API is healthy or not, specially after a new deploy.

6. There is no authentication due lack of context on how the API will be used, so all routes are public.

7. There is no subscription to the Firestore to keep track of near real time updates. This also was not implemented due the time restriction but using this feature from the HackerNews API could help keep the cache up to date vefore the user send a request.

## 3. Cake setup 🎂

To setup [Cake](https://cakebuild.net/) and use it as interface to build, test and run this API, you should follow the following steps:

1. Create Tool Manifest with `dotnet new tool-manifest`
2. Install with `dotnet tool install Cake.Tool --version 1.2.0`
3. Install Coverlet as a global tool with `dotnet tool install --global coverlet.console`
4. Run any task with the command `dotnet cake "build.cake" --target="TASK_NAME" --verbosity=normal` where `TASK_NAME` is the name of the desired task (check the [build.cake](build.cake) file to see all available tasks)

⚠ **Attention**: If you want to run the `Sonar` task, you should set your own Sonar token in [`build.cake`](build.cake), line 27.
