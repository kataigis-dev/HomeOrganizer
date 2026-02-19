# Kubernetes Deployment Files

This directory contains Kubernetes deployment configurations for the HomeOrganizer microservices.

## Structure

```
k8s/
├── products-service/     # Helm chart for Products microservice
│   ├── Chart.yaml
│   ├── values.yaml
│   ├── README.md
│   └── templates/
│       ├── _helpers.tpl
│       ├── deployment.yaml
│       ├── service.yaml
│       ├── serviceaccount.yaml
│       ├── configmap.yaml
│       ├── ingress.yaml
│       └── hpa.yaml
└── README.md            # This file
```

## Available Services

### Products Service

The Products microservice handles product management functionality.

**Location:** `products-service/`

**Docker Image Build:**
```bash
# From repository root
docker build -f Products.Api/Dockerfile -t products-api:latest .
```

**Deployment:**
```bash
# Install with Helm
helm install products products-service/

# Upgrade existing deployment
helm upgrade products products-service/

# Uninstall
helm uninstall products
```

For detailed configuration options, see [products-service/README.md](products-service/README.md).

## Prerequisites

- Kubernetes cluster (1.19+)
- kubectl configured to access your cluster
- Helm 3.0+
- Docker for building images

## Quick Start

1. **Build the Docker image:**
   ```bash
   cd /path/to/HomeOrganizer
   docker build -f Products.Api/Dockerfile -t products-api:latest .
   ```

2. **Push to your registry (if using remote cluster):**
   ```bash
   docker tag products-api:latest your-registry/products-api:latest
   docker push your-registry/products-api:latest
   ```

3. **Deploy with Helm:**
   ```bash
   helm install products k8s/products-service/ \
     --set image.repository=your-registry/products-api \
     --set image.tag=latest
   ```

4. **Verify deployment:**
   ```bash
   kubectl get pods
   kubectl get services
   ```

## Development

For local development with minikube or kind:

```bash
# Start minikube
minikube start

# Build image for minikube
eval $(minikube docker-env)
docker build -f Products.Api/Dockerfile -t products-api:latest .

# Deploy
helm install products k8s/products-service/ \
  --set image.pullPolicy=Never
```

## Configuration

Each service has its own `values.yaml` file with configurable parameters. Common configurations include:

- Replica count
- Resource limits and requests
- Environment variables
- Image repository and tag
- Service type and ports
- Ingress settings
- Autoscaling parameters

## Monitoring

Check deployment status:
```bash
kubectl get all -l app.kubernetes.io/name=products-service
kubectl logs -l app.kubernetes.io/name=products-service
kubectl describe pod -l app.kubernetes.io/name=products-service
```

## Troubleshooting

### Pod not starting
```bash
kubectl describe pod <pod-name>
kubectl logs <pod-name>
```

### Image pull errors
Ensure your image is pushed to the registry and pull secrets are configured if using a private registry.

### Service not accessible
Check service and pod status:
```bash
kubectl get svc
kubectl get endpoints
```

## Future Services

Additional microservices (e.g., Expenses/Payments service) can be added following the same structure:

```
k8s/
├── products-service/
├── expenses-service/    # Future
└── other-service/       # Future
```
