using AutoMapper;
using Darmon.Application.DTOs;
using Darmon.Application.DTOs.AddressDtos;
using Darmon.Application.DTOs.BranchDtos;
using Darmon.Application.DTOs.CartItemDtos;
using Darmon.Application.DTOs.CategotyDtos;
using Darmon.Application.DTOs.CourierDtos;
using Darmon.Application.DTOs.DeliveryDtos;
using Darmon.Application.DTOs.OrderDtos;
using Darmon.Application.DTOs.ProductDTos;
using Darmon.Application.DTOs.SellerWalletDtos;
using Darmon.Application.DTOs.User;
using Darmon.Application.DTOs.WithdrawHistoryDtos;
using Darmon.Domain.Entities;
using Darmon.Domain.Entities.Enums;

namespace Darmon.Application.Mappings;

public class MappingProfil : Profile
{
    public MappingProfil()
    {
        CreateMap<Address, AddressDto>(); // Entity → DTO>
        CreateMap<CreateAddressDto, Address>(); // DTO → Entity
        CreateMap<UpdateAddressDto, Address>(); // DTO → Entity


        // 🧑‍💼 User mapping
        CreateMap<UserRequestDto, User>(); // DTO → Entity
        CreateMap<User, UserResponseDto>() // Entity → DTO
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        // 📦 Product mapping
        CreateMap<Product, ProductDto>(); // Entity → DTO
        CreateMap<CreateProductDto, Product>(); // DTO → Entity
        CreateMap<UpdateProductDto, Product>(); // DTO → Entity

        // 🗂️ Category mapping
        CreateMap<Category, CategoryDto>() // Entity → DTO
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));
        CreateMap<CreateCategoryDto, Category>(); // DTO → Entity
        CreateMap<UpdateCategoryDto, Category>(); // DTO → Entity

        // 🛒 CartItem mapping
        CreateMap<CartItem, CartItemDto>() // Entity → DTO
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price));
        CreateMap<CreateCartItemDto, CartItem>(); // DTO → Entity
        CreateMap<UpdateCartItemDto, CartItem>(); // DTO → Entity

        // 🚚 Courier mapping
        CreateMap<Courier, CourierDto>(); // Entity → DTO
        CreateMap<CreateCourierDto, Courier>(); // DTO → Entity
        CreateMap<UpdateCourierDto, Courier>(); // DTO → Entity

        // Courier Orders mapping (optional, if Orders are included in DTO)
        CreateMap<Courier, CourierDto>()
            .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders));

        // 📦 Order mapping
        CreateMap<Order, OrderDto>(); // Entity → DTO
        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderDto, Order>();

        CreateMap<CreateDeliveryDto, Delivery>();
        CreateMap<UpdateDeliveryDto, Delivery>();
        CreateMap<Delivery, DeliveryDto>();

        CreateMap<CreateBranchDto, Branch>();
        CreateMap<UpdateBranchDto, Branch>();
        CreateMap<Branch, BranchDto>();

        // 💰 SellerWallet mapping
        CreateMap<SellerWallet, SellerWalletDto>() // Entity → DTO
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
        CreateMap<CreateSellerWalletDto, SellerWallet>(); // DTO → Entity

        // 💸 WithdrawHistory mapping
        CreateMap<WithdrawHistory, WithdrawHistoryDto>(); // Entity → DTO
        CreateMap<CreateWithdrawHistoryDto, WithdrawHistory>(); // DTO → Entity

        // Withdraw status update mapping
        CreateMap<UpdateWithdrawStatusDto, WithdrawHistory>() // DTO → Entity
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));



     


    }
}
