import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FamilyMemberResponse, CreateFamilyMemberRequest } from '../models/family.dto';

@Injectable({
  providedIn: 'root'
})
export class FamilyApiService {
  private readonly baseUrl = '/settings/family';

  constructor(private http: HttpClient) {}

  getFamilyMembers(): Observable<FamilyMemberResponse[]> {
    return this.http.get<FamilyMemberResponse[]>(this.baseUrl);
  }

  getFamilyMember(id: string): Observable<FamilyMemberResponse> {
    return this.http.get<FamilyMemberResponse>(`${this.baseUrl}/${id}`);
  }

  createFamilyMember(request: CreateFamilyMemberRequest): Observable<string> {
    return this.http.post<string>(this.baseUrl, request);
  }

  addAllergy(familyMemberId: string, allergen: string, severity: 'AvailableForOthers' | 'NotAllowed'): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${familyMemberId}/allergies`, { allergen, severity });
  }

  removeAllergy(familyMemberId: string, allergen: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${familyMemberId}/allergies/${allergen}`);
  }

  addSafeFood(familyMemberId: string, foodItem: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${familyMemberId}/safefoods`, { foodItem });
  }

  removeSafeFood(familyMemberId: string, foodItem: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${familyMemberId}/safefoods/${foodItem}`);
  }

  addFoodPreference(
    familyMemberId: string, 
    foodItem: string, 
    status: 'Include' | 'AvailableForOthers' | 'NotAllowed'
  ): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${familyMemberId}/preferences`, { foodItem, status });
  }

  removeFoodPreference(familyMemberId: string, foodItem: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${familyMemberId}/preferences/${foodItem}`);
  }
} 