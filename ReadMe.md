Command to start Prometheus docker

docker run -it --rm -p 9090:9090 -v C://tmp/prometheus.yml:/etc/prometheus/prometheus.yml prom/prometheus

docker run -it --rm -p 3000:3000 --name grafana grafana/grafana

import prometheus-net grafana dashboard - 10427


# Set up Kibana and elastic search

## spin up elastic search
docker network create elastic
docker run  -it --rm --name es01-test --net elastic -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" elasticsearch:7.12.1

## spin up kibana
docker run --name kib01-test --net elastic -p 5601:5601 -e "ELASTICSEARCH_HOSTS=http://es01-test:9200" kibana:7.12.1

