import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { of, throwError, Subject } from 'rxjs';
import { FamilyApiService } from '../services/family-api.service';
import { FamilyMembersStore } from './family-members.store';
import { CreateFamilyMemberRequest, FamilyMemberResponse } from '../models/family.dto';
import { FamilyMember, AllergenSeverity, FoodPreferenceStatus } from '../models/family.model';
import { mockProvider } from '@ngneat/spectator/jest';
import type { SpyObject } from '@ngneat/spectator/jest';
import { provideTestLogger } from '../../common/logging/test-logger';
import { dataLoadedStatus, dataLoadingStatus, dataErrorStatus, initialDataLoadingStatus } from '../../common/models/data-loading-status';
import { fakeAsync, tick, flushMicrotasks } from '@angular/core/testing';

describe('FamilyMembersStore', () => {
  let spectator: SpectatorService<FamilyMembersStore>;
  let store: FamilyMembersStore;
  let apiService: SpyObject<FamilyApiService>;

  const mockFamilyMember: FamilyMember = {
    id: '123',
    name: 'John Doe',
    age: 30,
    allergies: [{ allergen: 'peanuts', severity: AllergenSeverity.NotAllowed }],
    safeFoods: [{ foodItem: 'pasta' }],
    foodPreferences: [{ foodItem: 'rice', status: FoodPreferenceStatus.Include }]
  };

  const createService = createServiceFactory({
    service: FamilyMembersStore,
    providers: [
      mockProvider(FamilyApiService, {
        getFamilyMembers: jest.fn().mockReturnValue(of([])),
        createFamilyMember: jest.fn().mockReturnValue(of('')),
        addAllergy: jest.fn().mockReturnValue(of(void 0)),
        removeAllergy: jest.fn().mockReturnValue(of(void 0)),
        addSafeFood: jest.fn().mockReturnValue(of(void 0)),
        removeSafeFood: jest.fn().mockReturnValue(of(void 0)),
        addFoodPreference: jest.fn().mockReturnValue(of(void 0)),
        removeFoodPreference: jest.fn().mockReturnValue(of(void 0))
      }),
      provideTestLogger(true),
    ]
  });

  beforeEach(() => {
    spectator = createService();
    store = spectator.service;
    apiService = spectator.inject(FamilyApiService);

    jest.clearAllMocks();
  });

  it('can be created', () => {
    expect(store).toBeTruthy();
  });

  describe('loadFamilyMembers', () => {
    it('loads family members successfully', () => {
      apiService.getFamilyMembers.mockReturnValueOnce(of([mockFamilyMember]));

      store.loadFamilyMembers();

      expect(apiService.getFamilyMembers).toHaveBeenCalled();
      expect(store.familyMembers()).toEqual([mockFamilyMember]);
      expect(store.getLoadStatus()).toEqual(dataLoadedStatus);
    });

    it('handles error when loading family members fails', () => {
      const error = new Error('Failed to load');
      apiService.getFamilyMembers.mockReturnValue(throwError(() => error));

      store.loadFamilyMembers();

      expect(store.familyMembers()).toEqual([]);
      expect(store.getLoadStatus()).toEqual(dataErrorStatus(error));
    });
  });

  describe('createFamilyMember', () => {
    it('transitions through loading states correctly', () => {
      const request: CreateFamilyMemberRequest = {
        name: 'John Doe',
        age: 30
      };

      const createResponse$ = new Subject<string>();
      const loadResponse$ = new Subject<FamilyMember[]>();
      
      apiService.createFamilyMember.mockReturnValue(createResponse$);
      apiService.getFamilyMembers.mockReturnValue(loadResponse$);

      // Initial state
      expect(store.getCreateStatus()).toEqual(initialDataLoadingStatus);

      // Start creation
      store.createFamilyMember(request);

      // Should be in loading state
      expect(store.getCreateStatus()).toEqual(dataLoadingStatus);

      // Complete creation
      createResponse$.next('123');
      createResponse$.complete();

      // Load updated list
      loadResponse$.next([mockFamilyMember]);
      loadResponse$.complete();

      // Should be in loaded state
      expect(store.getCreateStatus()).toEqual(dataLoadedStatus);
    });

    it('creates family member and reloads list', () => {
      const request: CreateFamilyMemberRequest = {
        name: 'John Doe',
        age: 30
      };

      apiService.createFamilyMember.mockReturnValue(of('123'));
      apiService.getFamilyMembers.mockReturnValue(of([mockFamilyMember]));

      store.createFamilyMember(request);

      expect(apiService.createFamilyMember).toHaveBeenCalledWith(request);
      expect(apiService.getFamilyMembers).toHaveBeenCalled();
      expect(store.getCreateStatus()).toEqual(dataLoadedStatus);
    });

    it('handles error when creating family member fails', () => {
      const error = new Error('Failed to create');
      const request: CreateFamilyMemberRequest = {
        name: 'John Doe',
        age: 30
      };

      apiService.createFamilyMember.mockReturnValue(throwError(() => error));

      store.createFamilyMember(request);

      expect(store.getCreateStatus()).toEqual(dataErrorStatus(error));
    });
  });

  describe('selectFamilyMember', () => {
    it('selects family member by id', () => {
      apiService.getFamilyMembers.mockReturnValue(of([mockFamilyMember]));
      
      store.loadFamilyMembers();
      store.selectFamilyMember(mockFamilyMember.id);
      
      expect(store.selectedFamilyMember()).toEqual(mockFamilyMember);
    });
  });

  describe('addAllergy', () => {
    it('adds allergy and reloads family members', () => {
      const params = {
        familyMemberId: '123',
        allergen: 'peanuts',
        severity: AllergenSeverity.NotAllowed
      };

      apiService.addAllergy.mockReturnValue(of(void 0));
      apiService.getFamilyMembers.mockReturnValue(of([mockFamilyMember]));

      store.addAllergy(params);

      expect(apiService.addAllergy).toHaveBeenCalledWith(
        params.familyMemberId,
        params.allergen,
        params.severity
      );
      expect(apiService.getFamilyMembers).toHaveBeenCalled();
      expect(store.getAllergyStatus()).toEqual(dataLoadedStatus);
      expect(store.familyMembers()).toEqual([mockFamilyMember]);
    });

    it('handles error when adding allergy fails', () => {
      const error = new Error('Failed to add allergy');
      const params = {
        familyMemberId: '123',
        allergen: 'peanuts',
        severity: AllergenSeverity.NotAllowed
      };

      apiService.addAllergy.mockReturnValue(throwError(() => error));

      store.addAllergy(params);

      expect(store.getAllergyStatus()).toEqual(dataErrorStatus(error));
    });
  });

  describe('addSafeFood', () => {
    it('adds safe food and reloads family members', () => {
      const params = {
        familyMemberId: '123',
        foodItem: 'pasta'
      };

      apiService.addSafeFood.mockReturnValue(of(void 0));
      apiService.getFamilyMembers.mockReturnValue(of([mockFamilyMember]));

      store.addSafeFood(params);

      expect(apiService.addSafeFood).toHaveBeenCalledWith(
        params.familyMemberId,
        params.foodItem
      );
      expect(apiService.getFamilyMembers).toHaveBeenCalled();
      expect(store.getSafeFoodStatus()).toEqual(dataLoadedStatus);
      expect(store.familyMembers()).toEqual([mockFamilyMember]);
    });

    it('handles error when adding safe food fails', () => {
      const error = new Error('Failed to add safe food');
      const params = {
        familyMemberId: '123',
        foodItem: 'pasta'
      };

      apiService.addSafeFood.mockReturnValue(throwError(() => error));

      store.addSafeFood(params);

      expect(store.getSafeFoodStatus()).toEqual(dataErrorStatus(error));
    });
  });

  describe('addFoodPreference', () => {
    it('adds food preference and reloads family members', () => {
      const params = {
        familyMemberId: '123',
        foodItem: 'rice',
        status: FoodPreferenceStatus.Include
      };

      apiService.addFoodPreference.mockReturnValue(of(void 0));
      apiService.getFamilyMembers.mockReturnValue(of([mockFamilyMember]));

      store.addFoodPreference(params);

      expect(apiService.addFoodPreference).toHaveBeenCalledWith(
        params.familyMemberId,
        params.foodItem,
        params.status
      );
      expect(apiService.getFamilyMembers).toHaveBeenCalled();
      expect(store.getPreferenceStatus()).toEqual(dataLoadedStatus);
      expect(store.familyMembers()).toEqual([mockFamilyMember]);
    });

    it('handles error when adding food preference fails', () => {
      const error = new Error('Failed to add food preference');
      const params = {
        familyMemberId: '123',
        foodItem: 'rice',
        status: FoodPreferenceStatus.Include
      };

      apiService.addFoodPreference.mockReturnValue(throwError(() => error));

      store.addFoodPreference(params);

      expect(store.getPreferenceStatus()).toEqual(dataErrorStatus(error));
    });
  });

  describe('concurrent operations', () => {
    it('demonstrates operation cancellation and mixed success/failure states', fakeAsync(() => {
      // Setup delayed responses
      const firstLoadResponse$ = new Subject<FamilyMemberResponse[]>();
      const secondLoadResponse$ = new Subject<FamilyMemberResponse[]>();
      const allergyResponse$ = new Subject<void>();
      
      // Mock the API calls to use our subjects
      apiService.getFamilyMembers
        .mockReturnValueOnce(firstLoadResponse$)
        .mockReturnValueOnce(secondLoadResponse$);
      apiService.addAllergy.mockReturnValue(allergyResponse$);

      // Start first load
      store.loadFamilyMembers();
      tick();
      expect(store.getLoadStatus()).toEqual(dataLoadingStatus);

      // Start allergy operation - should be independent
      store.addAllergy({ familyMemberId: '123', allergen: 'peanuts', severity: AllergenSeverity.NotAllowed });
      tick();
      expect(store.getAllergyStatus()).toEqual(dataLoadingStatus);

      // Start second load - should cancel first load
      store.loadFamilyMembers();
      tick();
      
      // Complete second load successfully
      secondLoadResponse$.next([mockFamilyMember]);
      secondLoadResponse$.complete();
      tick();
      expect(store.getLoadStatus()).toEqual(dataLoadedStatus);

      // Fail the allergy operation
      const error = new Error('Failed to add allergy');
      allergyResponse$.error(error);
      tick();
      expect(store.getAllergyStatus()).toEqual(dataErrorStatus(error));

      // Verify first load was cancelled by completing it - should have no effect
      firstLoadResponse$.next([]);
      firstLoadResponse$.complete();
      tick();
      
      // Final state verification
      expect(store.getLoadStatus()).toEqual(dataLoadedStatus);
      expect(store.getAllergyStatus()).toEqual(dataErrorStatus(error));
      expect(store.familyMembers()).toEqual([mockFamilyMember]);
    }));
  });
}); 