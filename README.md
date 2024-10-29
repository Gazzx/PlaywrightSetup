# About

This is a small console program to install Playwright's dotnet version's dependencies and browsers.
The advertised way is cumbersome and requires powershell, making it needlessly hard to deploy Playwright's dotnet version to production.

This small project should help. It is a [20 lines code program](https://github.com/asfaload/PlaywrightSetup/blob/master/Program.fs),
of which only 2 take action: [install dependencies](https://github.com/asfaload/PlaywrightSetup/blob/master/Program.fs#L5) and [install browsers](https://github.com/asfaload/PlaywrightSetup/blob/master/Program.fs#L12), making it easy to audit.

# Origin
At [Asfaload](https://www.asfaload.com) we use Playwright for some web automation, and got tired of the problematic playwright installation instructions.
[The Github issue](https://github.com/microsoft/playwright-dotnet/issues/2286) asking for a better installation procedure is the project's top-voted one, showing there's a general consensus that there should be a better way. This could be a better way until it gets fixed upstream.

# Using it

It has been developed to be usable in containers, and supports [multi-stage builds](https://docs.docker.com/build/building/multi-stage/).

## Dockerfile
Here is an example of a `Dockerfile`:

```
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Clone the project
RUN git clone https://github.com/asfaload/PlaywrightSetup.git /PlaywrightSetup
WORKDIR /PlaywrightSetup
# Build the program
RUN dotnet publish
# Then run it
RUN /PlaywrightSetup/PlaywrightSetup

# Add your normal image building instructions here

```

## Multi-stage builds
Multi-stage builds let you build leaner images to be deployed in production.
You typically compile your dotnet code with the `sdk` image but deploy it with the `runtime` image.
Here's how you can you `PlaywrightSetup` in a multi-stage build Dockerfile:

```
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
RUN git clone https://github.com/asfaload/PlaywrightSetup.git /PlaywrightSetup
WORKDIR /PlaywrightSetup
RUN dotnet publish
# Add your build instructions here

#----------------------------------------
FROM mcr.microsoft.com/dotnet/runtime:8.0
# copy PlaywrightSetup from the build image
COPY --from=builder /PlaywrightSetup/bin/Release/net8.0/publish /PlaywrightSetup
# Then run it. This needs to run as root, as it installs dependencies.
RUN /PlaywrightSetup/PlaywrightSetup
# Clean up, we don't need to deploy this
RUN rm -rf /PlaywrightSetup

# Add your instructions here
```
