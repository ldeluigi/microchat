# Microchat
[![Microchat CI/CD](https://github.com/ldeluigi/microchat/actions/workflows/main.ci-cd.yml/badge.svg?branch=master&event=push)](https://github.com/ldeluigi/microchat/actions/workflows/main.ci-cd.yml)
[![Microservices CI](https://github.com/ldeluigi/microchat/actions/workflows/backend.ci.yml/badge.svg?branch=master&event=push)](https://github.com/ldeluigi/microchat/actions/workflows/backend.ci.yml)
[![Angular CI](https://github.com/ldeluigi/microchat/actions/workflows/frontend.ci.yml/badge.svg?branch=master&event=push)](https://github.com/ldeluigi/microchat/actions/workflows/frontend.ci.yml)
[![Update GitHub Pages](https://github.com/ldeluigi/microchat/actions/workflows/gh-pages.yml/badge.svg?branch=master&event=push)](https://ldeluigi.github.io/microchat/)
[![Mirror Repository](https://github.com/ldeluigi/microchat/actions/workflows/mirror.yml/badge.svg?branch=master&event=push)](https://gitlab.com/pika-lab/courses/ds/projects/ds-project-angelini-deluigi-magnani-ay2021)

The documentation for this project can be found at:
https://ldeluigi.github.io/microchat


# Demo Development

## Needed on the host

- [Angular CLI](https://github.com/angular/angular-cli) v13.
- [NodeJS](https://nodejs.org/) v16.
- [Docker](https://www.docker.com/) engine version v20.

## Backend Development

To deploy backend it's necessary go in its folder `cd .\services\` and start the docker compose `docker compose up -d --build`

### Stopping containers

To properly stop running containers use `docker compose down`.
To stop and **clean volumes** use `docker compose down -v`.  
**Note:** _Volumes left inside docker storage could fill up the space on the host machine!_

## Frontend Development

The client is developed and compiled on the host machine, so you need to go on its folder `cd .\frontend\microchat-frontend\` and run `npm install` to download dependencies; then u can run frontend using `ng serve`.
**Note:** _ng serve is a blocking call_

Navigate to `http://localhost:4200/` to use the frontend.