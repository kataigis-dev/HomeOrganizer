# Products Service Helm Chart

This Helm chart deploys the Products microservice from the HomeOrganizer application to Kubernetes.

## Prerequisites

- Kubernetes 1.19+
- Helm 3.0+
- Docker image built and available in your registry

## Building the Docker Image

Before deploying, build and push the Docker image:

```bash
# From the repository root
docker build -f Products.Api/Dockerfile -t products-api:latest .

# Tag and push to your registry
docker tag products-api:latest your-registry/products-api:latest
docker push your-registry/products-api:latest
```

## Installing the Chart

To install the chart with the release name `products`:

```bash
# From the repository root
helm install products k8s/products-service/

# Or with custom values
helm install products k8s/products-service/ -f custom-values.yaml
```

## Uninstalling the Chart

To uninstall/delete the `products` deployment:

```bash
helm uninstall products
```

## Configuration

The following table lists the configurable parameters of the Products Service chart and their default values.

| Parameter | Description | Default |
|-----------|-------------|---------|
| `replicaCount` | Number of replicas | `2` |
| `image.repository` | Image repository | `products-api` |
| `image.tag` | Image tag | `latest` |
| `image.pullPolicy` | Image pull policy | `IfNotPresent` |
| `service.type` | Kubernetes service type | `ClusterIP` |
| `service.port` | Service port | `80` |
| `service.targetPort` | Container port | `8080` |
| `ingress.enabled` | Enable ingress | `false` |
| `resources.limits.cpu` | CPU limit | `500m` |
| `resources.limits.memory` | Memory limit | `512Mi` |
| `resources.requests.cpu` | CPU request | `250m` |
| `resources.requests.memory` | Memory request | `256Mi` |
| `autoscaling.enabled` | Enable horizontal pod autoscaling | `false` |
| `autoscaling.minReplicas` | Minimum number of replicas | `2` |
| `autoscaling.maxReplicas` | Maximum number of replicas | `10` |

## Examples

### Install with custom image

```bash
helm install products k8s/products-service/ \
  --set image.repository=your-registry/products-api \
  --set image.tag=v1.0.0
```

### Enable ingress

```bash
helm install products k8s/products-service/ \
  --set ingress.enabled=true \
  --set ingress.hosts[0].host=products.example.com \
  --set ingress.hosts[0].paths[0].path=/ \
  --set ingress.hosts[0].paths[0].pathType=Prefix
```

### Enable autoscaling

```bash
helm install products k8s/products-service/ \
  --set autoscaling.enabled=true \
  --set autoscaling.minReplicas=2 \
  --set autoscaling.maxReplicas=10
```

### Upgrade existing deployment

```bash
helm upgrade products k8s/products-service/ \
  --set image.tag=v1.1.0
```

## Health Checks

The chart includes liveness and readiness probes that check `/health` endpoint. Make sure your application exposes this endpoint or modify the probe configuration in `values.yaml`.

## Notes

- The chart uses a ConfigMap to provide `appsettings.json` to the application
- Security context is configured to run as non-root user (UID 1000)
- Resource limits are set by default to prevent resource exhaustion
- The service account is created automatically unless disabled

## Customizing Configuration

Create a custom `values.yaml` file:

```yaml
replicaCount: 3

image:
  repository: myregistry/products-api
  tag: "1.0.0"

resources:
  limits:
    cpu: 1000m
    memory: 1Gi
  requests:
    cpu: 500m
    memory: 512Mi

env:
  - name: ASPNETCORE_ENVIRONMENT
    value: "Production"
  - name: DATABASE_CONNECTION_STRING
    valueFrom:
      secretKeyRef:
        name: products-secrets
        key: connection-string
```

Then install with:

```bash
helm install products k8s/products-service/ -f custom-values.yaml
```
