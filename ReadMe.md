Command to start Prometheus docker

docker run -it --rm -p 9090:9090 -v C://tmp/prometheus.yml:/etc/prometheus/prometheus.yml prom/prometheus