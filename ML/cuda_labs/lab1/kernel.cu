#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <time.h>



static void CatchError(cudaError_t err, const char* file, int line) {
    if (err != cudaSuccess) {
        printf("%s in %s at line %d\n", cudaGetErrorString(err),
            file, line);
        exit(EXIT_FAILURE);
    }
}
#define CATCH_ERROR( err ) (CatchError( err, __FILE__, __LINE__ ))


__global__ void kernel(double *res, double *array_degrees, long long int n)
{
    long long int tid = blockIdx.x * blockDim.x + threadIdx.x;
    long long int offset = blockDim.x * gridDim.x;
    
    while (tid < n) {
        // writing to array result of calculations with conversion to degrees
        res[tid] = cosf( array_degrees[tid%90] * 3.141592653589 / 180 );
        tid += offset;
    }
}

void my_cos(double *res, double *array_degrees, long long int n) {
    long long int tid = 0;

    while (tid < n) {
        // writing to array result of calculations with conversion to degrees
        res[tid] = cosf(array_degrees[tid % 90] * 3.141592653589 / 180);
        tid += 1;
    }
}

int main()
{
    int count;
    CATCH_ERROR( cudaGetDeviceCount( &count ) );
    if (count == 0) {
        printf("there is no cuda device");
        return -1;
    }

    long long int n = 1000000;

    // for timer
    cudaEvent_t time_of_start, time_of_end;
    float res_timer_gpu;
    CATCH_ERROR( cudaEventCreate( &time_of_start ) );
    CATCH_ERROR( cudaEventCreate( &time_of_end ) );
    


    double array_degrees[90];    // array with degrees
    for (int i = 0; i < 90; i++) {  //  from 0 to 90
        array_degrees[i] = i;
    }

    double *res = (double*)malloc(n * sizeof(double));
    double *ar_d_dev, *res_dev;
    
    CATCH_ERROR( cudaMalloc( &res_dev, n * sizeof(double) ) );
    CATCH_ERROR( cudaMalloc( &ar_d_dev, 90 * sizeof(double) ) );

    CATCH_ERROR( cudaMemcpy( ar_d_dev, array_degrees, 90 * sizeof(double), cudaMemcpyHostToDevice ) );
    

    CATCH_ERROR( cudaEventRecord( time_of_start ) ); // start of timer (GPU)

    kernel <<<256,256>>>(res_dev, ar_d_dev, n);

    CATCH_ERROR( cudaEventRecord( time_of_end )); // end of timer (GPU)
    CATCH_ERROR( cudaEventSynchronize( time_of_end ) );
    
    CATCH_ERROR( cudaEventElapsedTime( &res_timer_gpu, time_of_start, time_of_end ) );
    
    CATCH_ERROR( cudaEventDestroy( time_of_start ) );
    CATCH_ERROR( cudaEventDestroy( time_of_end ) );

    CATCH_ERROR( cudaMemcpy( res, res_dev, n * sizeof(double), cudaMemcpyDeviceToHost ) );
    
    CATCH_ERROR( cudaFree( ar_d_dev ) );
    CATCH_ERROR( cudaFree( res_dev ) );

    for (long long i = 0; i < n; i++) {
        printf("%f\n", res[i]);
    }

    free(res);

    res = (double*)malloc(n * sizeof(double));
    double res_timer_cpu = 0.0;

    clock_t begin = clock();
    
    my_cos(res, array_degrees, n);

    clock_t end = clock();

    res_timer_cpu += (double)(end - begin) / CLOCKS_PER_SEC;

    printf("Results by CPU: %f\n", res_timer_cpu*1000);

    printf("Results by GPU: %f\n", res_timer_gpu);
    
    free(res);
    return 0;
}