import { patchState, signalStoreFeature, withMethods, withState } from '@ngrx/signals';
import { Observable, tap, catchError, of } from 'rxjs';
import { AppLogger } from '../logging/app-logger.service';
import { DataLoadingStatus, dataLoadedStatus, dataLoadingStatus, dataErrorStatus, initialDataLoadingStatus } from '../models/data-loading-status';
import { computed, inject } from '@angular/core';

export interface ApiCallState {
    _apiCallStatuses: Record<string, DataLoadingStatus>;
}

const initialState: ApiCallState = {
    _apiCallStatuses: {}
};

interface ApiCallParams<T> {
    key: string;
    apiCall: Observable<T>;
    onSuccess: (response: T) => void;
}

export function withApiHandling() {
    return signalStoreFeature(
        withState(initialState),
        withMethods((store, logger = inject(AppLogger)) => ({
            _getCallStatus: (key: string) => computed(() => store._apiCallStatuses()[key] ?? initialDataLoadingStatus),
            
            _handleApiCall: <T>(params: ApiCallParams<T>) => {
                logger.info(`Starting API call: ${params.key}`);
                patchState(store, {
                    _apiCallStatuses: {
                        ...store._apiCallStatuses(),
                        [params.key]: dataLoadingStatus
                    }
                });

                return params.apiCall.pipe(
                    tap(response => {
                        logger.info(`API call successful: ${params.key}`);
                        params.onSuccess(response);
                        patchState(store, {
                            _apiCallStatuses: {
                                ...store._apiCallStatuses(),
                                [params.key]: dataLoadedStatus
                            }
                        });
                    }),
                    catchError(error => {
                        logger.error(`API call failed: ${params.key}`, error);
                        patchState(store, {
                            _apiCallStatuses: {
                                ...store._apiCallStatuses(),
                                [params.key]: dataErrorStatus(error)
                            }
                        });
                        return of(undefined);
                    })
                );
            }
        }))
    );
} 