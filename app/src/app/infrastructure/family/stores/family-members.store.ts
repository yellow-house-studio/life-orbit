import { computed, inject, Injectable } from '@angular/core';
import { signalStore, withState, withComputed, withMethods, patchState, withHooks } from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { pipe, switchMap, tap, catchError, of, Observable, throwError, finalize } from 'rxjs';

import { FamilyMember } from '../models/family.model';
import { CreateFamilyMemberRequest } from '../models/family.dto';
import { FamilyApiService } from '../services/family-api.service';
import { mapFamilyMemberResponseToModel } from '../models/family.mapper';
import { AppLogger } from '../../common/logging/app-logger.service';

export interface FamilyMembersState {
    familyMembers: FamilyMember[];
    selectedFamilyMemberId: string | null;
    loading: boolean;
    error: string | null;
}

const initialState: FamilyMembersState = {
    familyMembers: [],
    selectedFamilyMemberId: null,
    loading: false,
    error: null
};

@Injectable({ providedIn: 'root' })
export class FamilyMembersStore extends signalStore(
    withState<FamilyMembersState>(initialState),
    withComputed((state) => ({
        selectedFamilyMember: computed(() =>
            state.familyMembers().find(member => member.id === state.selectedFamilyMemberId())
        )
    })),
    withMethods((store, apiService = inject(FamilyApiService)) => {
        const logger = inject(AppLogger).forContext('FamilyMembersStore');
        const loadFamilyMembers = rxMethod<void>(
            pipe(
                tap(() => {
                    logger.info('Loading family members');
                    patchState(store, { loading: true, error: null });
                }),
                switchMap(() =>
                    apiService.getFamilyMembers().pipe(
                        tap(response => {
                            logger.info('Family members loaded successfully');
                            const mappedMembers = response.map(mapFamilyMemberResponseToModel);
                            patchState(store, {
                                familyMembers: mappedMembers,
                                loading: false
                            });
                        }),
                        catchError(error => {
                            logger.error('Error loading family members', error);
                            patchState(store, { error: error.message, loading: false });
                            return of(undefined);
                        }),
                        finalize(() => {
                            patchState(store, { loading: false });
                        })
                    )
                )
            )
        );

        return {
            loadFamilyMembers,
            selectFamilyMember: (id: string) => {
                patchState(store, { selectedFamilyMemberId: id });
            },

            createFamilyMember: rxMethod<CreateFamilyMemberRequest>(
                pipe(
                    tap(() => patchState(store, { loading: true, error: null })),
                    switchMap(request =>
                        apiService.createFamilyMember(request).pipe(
                            tap(() => {
                                loadFamilyMembers();
                            }),
                            catchError(error => {
                                patchState(store, { error: error.message, loading: false });
                                return throwError(() => error);
                            }),
                            finalize(() => {
                                patchState(store, { loading: false });
                            })
                        )
                    )
                )
            ),

            addAllergy: rxMethod<{ familyMemberId: string; allergen: string; severity: 'AvailableForOthers' | 'NotAllowed' }>(
                pipe(
                    tap(() => patchState(store, { loading: true, error: null })),
                    switchMap(({ familyMemberId, allergen, severity }) =>
                        apiService.addAllergy(familyMemberId, allergen, severity).pipe(
                            tap(() => loadFamilyMembers()),
                            catchError(error => {
                                patchState(store, { error: error.message });
                                return throwError(() => error);
                            }),
                            finalize(() => {
                                patchState(store, { loading: false });
                            })
                        )
                    )
                )
            ),

            removeAllergy: rxMethod<{ familyMemberId: string; allergen: string }>(
                pipe(
                    tap(() => patchState(store, { loading: true, error: null })),
                    switchMap(({ familyMemberId, allergen }) =>
                        apiService.removeAllergy(familyMemberId, allergen).pipe(
                            tap(() => loadFamilyMembers()),
                            catchError(error => {
                                patchState(store, { error: error.message });
                                return throwError(() => error);
                            }),
                            finalize(() => {
                                patchState(store, { loading: false });
                            })
                        )
                    )
                )
            ),

            addSafeFood: rxMethod<{ familyMemberId: string; foodItem: string }>(
                pipe(
                    tap(() => patchState(store, { loading: true, error: null })),
                    switchMap(({ familyMemberId, foodItem }) =>
                        apiService.addSafeFood(familyMemberId, foodItem).pipe(
                            tap(() => loadFamilyMembers()),
                            catchError(error => {
                                patchState(store, { error: error.message });
                                return throwError(() => error);
                            }),
                            finalize(() => {
                                patchState(store, { loading: false });
                            })
                        )
                    )
                )
            ),

            removeSafeFood: rxMethod<{ familyMemberId: string; foodItem: string }>(
                pipe(
                    tap(() => patchState(store, { loading: true, error: null })),
                    switchMap(({ familyMemberId, foodItem }) =>
                        apiService.removeSafeFood(familyMemberId, foodItem).pipe(
                            tap(() => loadFamilyMembers()),
                            catchError(error => {
                                patchState(store, { error: error.message });
                                return throwError(() => error);
                            }),
                            finalize(() => {
                                patchState(store, { loading: false });
                            })
                        )
                    )
                )
            ),

            addFoodPreference: rxMethod<{
                familyMemberId: string;
                foodItem: string;
                status: 'Include' | 'AvailableForOthers' | 'NotAllowed'
            }>(
                pipe(
                    tap(() => patchState(store, { loading: true, error: null })),
                    switchMap(({ familyMemberId, foodItem, status }) =>
                        apiService.addFoodPreference(familyMemberId, foodItem, status).pipe(
                            tap(() => loadFamilyMembers()),
                            catchError(error => {
                                patchState(store, { error: error.message });
                                return throwError(() => error);
                            }),
                            finalize(() => {
                                patchState(store, { loading: false });
                            })
                        )
                    )
                )
            ),

            removeFoodPreference: rxMethod<{ familyMemberId: string; foodItem: string }>(
                pipe(
                    tap(() => patchState(store, { loading: true, error: null })),
                    switchMap(({ familyMemberId, foodItem }) =>
                        apiService.removeFoodPreference(familyMemberId, foodItem).pipe(
                            tap(() => loadFamilyMembers()),
                            catchError(error => {
                                patchState(store, { error: error.message });
                                return throwError(() => error);
                            }),
                            finalize(() => {
                                patchState(store, { loading: false });
                            })
                        )
                    )
                )
            )
        };
    }),
    withHooks({
        onInit(store) {
            store.loadFamilyMembers();
        }
    })
) { }