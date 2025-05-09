#!/bin/bash

set -e


server_up() {
    echo "Server up..."
    docker pull zvezdun/ramp_rage:latest
    docker stop ramp_rage_backend || true
    docker rm ramp_rage_backend || true
    docker run -d --restart=always -v /volumes/ramp_rage/images:/app/uploads --name ramp_rage_backend -p 7096:8080 zvezdun/ramp_rage:latest
}


start_containers() {
    echo "Containers start..."
    docker run -d --restart=always -v /volumes/ramp_rage/images:/app/uploads --name ramp_rage_backend -p 7096:8080 zvezdun/ramp_rage:latest
}

stop_containers() {
    echo "Containers stop..."
    docker stop ramp_rage_backend || true
    docker rm ramp_rage_backend || true
}

restart_containers() {
    echo "Containers restart..."
    docker stop ramp_rage_backend || true
    docker rm ramp_rage_backend || true
    docker run -d --restart=always -v /volumes/ramp_rage/images:/app/uploads --name ramp_rage_backend -p 7096:8080 zvezdun/ramp_rage:latest
}

show_containers() {
	echo "Container show"
	docker ps -a
}

echo "Choose action:"
echo "1. Server up"
echo "2. Containers start"
echo "3. Containers stop"
echo "4. Containers restart"
echo "5. Containers show"
read -p "Enter action number: " action

case $action in
    1)
        server_up
        ;;
    2)
        start_containers
        ;;
    3)
        stop_containers
        ;;
    4)
        restart_containers
        ;;
	5)
		show_containers
		;;
    *)
        echo "Invalid action number!"
        exit 1
        ;;
esac