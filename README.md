# tumblepub

[![Build Status](https://cloud.drone.io/api/badges/sand-head/tumblepub/status.svg)](https://cloud.drone.io/sand-head/tumblepub)

an ActivityPub-federated tumbleblog hosting platform

## a quick note

if you're adding new queries or tables or whatever, make sure to update the offline cache by running `cargo sqlx prepare --merged`.
