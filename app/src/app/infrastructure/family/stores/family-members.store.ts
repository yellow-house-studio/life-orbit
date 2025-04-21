import { computed, inject, Injectable, input, signal } from '@angular/core';
import { signalStore, withState, withComputed, withMethods, patchState, withHooks } from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { pipe, switchMap, of, map } from 'rxjs';
import { FamilyMember, Allergy, SafeFood, FoodPreference } from '../models/family.model';
import { CreateCompleteFamilyMemberRequest, FamilyMemberResponse } from '../models/family.dto';
import { FamilyApiService } from '../services/family-api.service';
import { createFamilyMemeberRequest, mapFamilyMemberResponseToModel } from '../models/family.mapper';
import { withApiHandling } from '../../common/stores/with-api-handling';

export interface FamilyMembersState {
    familyMembers: FamilyMember[];
    selectedFamilyMemberId: string | null;
}

const initialState: FamilyMembersState = {
    familyMembers: [],
    selectedFamilyMemberId: null
};

// API call keys
const enum ApiCallKeys {
    LoadFamilyMembers = 'loadFamilyMembers',
    CreateCompleteFamilyMember = 'createCompleteFamilyMember',
    AddAllergy = 'addAllergy',
    RemoveAllergy = 'removeAllergy',
    AddSafeFood = 'addSafeFood',
    RemoveSafeFood = 'removeSafeFood',
    AddFoodPreference = 'addFoodPreference',
    RemoveFoodPreference = 'removeFoodPreference'
}

export interface CreateCompleteFamilyMember {
    name: string;
    age: number;
    allergies: Allergy[];
    safeFoods: SafeFood[];
    foodPreferences: FoodPreference[];
}

@Injectable({ providedIn: 'root' })
export class FamilyMembersStore extends signalStore(
    withState<FamilyMembersState>(initialState),
    withApiHandling(),
    withComputed((state) => ({
        selectedFamilyMember: computed(() =>
            state.familyMembers().find(member => member.id === state.selectedFamilyMemberId())
        )
    })),
    withMethods((store, apiService = inject(FamilyApiService)) => {
        // Define the load function that will be used in onSuccess callbacks
        const loadFamilyMembersCall = () => 
            store._handleApiCall<FamilyMemberResponse[]>({
                key: ApiCallKeys.LoadFamilyMembers,
                apiCall: apiService.getFamilyMembers(),
                onSuccess: (response) => {
                    const mappedMembers = response.map(mapFamilyMemberResponseToModel);
                    patchState(store, { familyMembers: mappedMembers });
                }
            });

        return {
            loadFamilyMembers: rxMethod<void>(
                pipe(
                    switchMap(() => loadFamilyMembersCall())
                )
            ),

            selectFamilyMember: rxMethod<string>(
                pipe(
                    switchMap((id) => {
                        patchState(store, { selectedFamilyMemberId: id });
                        return of(undefined);
                    })
                )
            ),

            createCompleteFamilyMember: rxMethod<CreateCompleteFamilyMember>(
                pipe(
                    map((input) => createFamilyMemeberRequest(
                        input.name,
                        input.age,
                        input.allergies,
                        input.safeFoods,
                        input.foodPreferences
                    )),
                    switchMap((request) => {
                        return store._handleApiCall<FamilyMemberResponse>({
                            key: ApiCallKeys.CreateCompleteFamilyMember,
                            apiCall: apiService.createCompleteFamilyMember(request),
                            onSuccess: (response) => {
                                const mappedMember = mapFamilyMemberResponseToModel(response);
                                return loadFamilyMembersCall().pipe(
                                    switchMap(() => of(mappedMember))
                                );
                            }
                        });
                    })
                )
            ),

            addAllergy: rxMethod<{ familyMemberId: string; allergen: string; severity: 'AvailableForOthers' | 'NotAllowed' }>(
                pipe(
                    switchMap(({ familyMemberId, allergen, severity }) =>
                        store._handleApiCall<void>({
                            key: ApiCallKeys.AddAllergy,
                            apiCall: apiService.addAllergy(familyMemberId, allergen, severity),
                            onSuccess: () => loadFamilyMembersCall()
                        })
                    )
                )
            ),

            removeAllergy: rxMethod<{ familyMemberId: string; allergen: string }>(
                pipe(
                    switchMap(({ familyMemberId, allergen }) =>
                        store._handleApiCall<void>({
                            key: ApiCallKeys.RemoveAllergy,
                            apiCall: apiService.removeAllergy(familyMemberId, allergen),
                            onSuccess: () => loadFamilyMembersCall()
                        })
                    )
                )
            ),

            addSafeFood: rxMethod<{ familyMemberId: string; foodItem: string }>(
                pipe(
                    switchMap(({ familyMemberId, foodItem }) =>
                        store._handleApiCall<void>({
                            key: ApiCallKeys.AddSafeFood,
                            apiCall: apiService.addSafeFood(familyMemberId, foodItem),
                            onSuccess: () => loadFamilyMembersCall()
                        })
                    )
                )
            ),

            removeSafeFood: rxMethod<{ familyMemberId: string; foodItem: string }>(
                pipe(
                    switchMap(({ familyMemberId, foodItem }) =>
                        store._handleApiCall<void>({
                            key: ApiCallKeys.RemoveSafeFood,
                            apiCall: apiService.removeSafeFood(familyMemberId, foodItem),
                            onSuccess: () => loadFamilyMembersCall()
                        })
                    )
                )
            ),

            addFoodPreference: rxMethod<{
                familyMemberId: string;
                foodItem: string;
                status: 'Include' | 'AvailableForOthers' | 'NotAllowed'
            }>(
                pipe(
                    switchMap(({ familyMemberId, foodItem, status }) =>
                        store._handleApiCall<void>({
                            key: ApiCallKeys.AddFoodPreference,
                            apiCall: apiService.addFoodPreference(familyMemberId, foodItem, status),
                            onSuccess: () => loadFamilyMembersCall()
                        })
                    )
                )
            ),

            removeFoodPreference: rxMethod<{ familyMemberId: string; foodItem: string }>(
                pipe(
                    switchMap(({ familyMemberId, foodItem }) =>
                        store._handleApiCall<void>({
                            key: ApiCallKeys.RemoveFoodPreference,
                            apiCall: apiService.removeFoodPreference(familyMemberId, foodItem),
                            onSuccess: () => loadFamilyMembersCall()
                        })
                    )
                )
            ),

            // Expose status getters using the computed selector from the feature
            getLoadStatus: store._getCallStatus(ApiCallKeys.LoadFamilyMembers),
            getCreateCompleteStatus: store._getCallStatus(ApiCallKeys.CreateCompleteFamilyMember),
            getAllergyStatus: store._getCallStatus(ApiCallKeys.AddAllergy),
            getSafeFoodStatus: store._getCallStatus(ApiCallKeys.AddSafeFood),
            getPreferenceStatus: store._getCallStatus(ApiCallKeys.AddFoodPreference)
        };
    }),
    withHooks({
        onInit(store) {
            store.loadFamilyMembers();
        }
    })
) { }