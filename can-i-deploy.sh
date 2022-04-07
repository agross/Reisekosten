#!/usr/bin/env bash

set -e

PACT_BROKER_BASE_URL=https://pact.grossweber.com

sha="$(git rev-parse HEAD)"
branch=$(git name-rev --name-only HEAD)

version="${sha:0:7}-$branch"

docker run \
       --rm \
       pactfoundation/pact-cli:latest \
       broker \
       can-i-deploy \
       --broker-base-url "$PACT_BROKER_BASE_URL" \
       --pacticipant "Reisekosten Backend" \
       --version "$version"
