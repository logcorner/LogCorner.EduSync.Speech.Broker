docker run -d -p 9090:9090 -v /path/to/prometheus/config:/etc/prometheus prom/prometheus
docker run -d --name=grafana -p 3000:3000 grafana/grafana-enterprise:10.0.0