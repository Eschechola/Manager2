using System.Threading.Tasks;
using AutoMapper;
using Manager.Core.Exceptions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Services.DTO;
using Manager.Services.interfaces;
using System.Collections.Generic;


namespace Manager.Services.Services{
    public class UserService : IUserService{
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        
        public async Task<UserDTO> Create(UserDTO userDTO)
        {
            var userExists = await _userRepository.GetByEmail(userDTO.Email);

            if(userExists != null)
                throw new DomainException("Já existe um usuário cadastrado com o email informado.");

            var user = _mapper.Map<User>(userDTO);
            user.Validate();

            var userCreated = await _userRepository.Create(user);

            return _mapper.Map<UserDTO>(userCreated);
        }
        public async Task<UserDTO> Update(UserDTO userDTO)
        {
            var userExists = await _userRepository.Get(userDTO.Id);

            if(userExists == null)
                throw new DomainException("Não existe nenhum usuário com o id informado.");

            var user = _mapper.Map<User>(userDTO);
            user.Validate();

            var userUpdated = await _userRepository.Update(user);

            return _mapper.Map<UserDTO>(userUpdated);
        }
        public async Task Remove(long id)
        {
            await _userRepository.Remove(id);
        }
        public async Task<UserDTO> Get(long Id)
        {
            var user = await _userRepository.Get();

            return _mapper.Map<UserDTO>(user);
        }
        public async Task<List<UserDTO>> Get()
        {
            var allUser = await _userRepository.Get();

            return _mapper.Map<List<UserDTO>>(allUser);
        }
        public async Task<List<UserDTO>> SearchByName(string name)
        {
            var allUser = await _userRepository.SearchByName(name);

            return _mapper.Map<List<UserDTO>>(allUser);
        }
        public async Task<List<UserDTO>> SearchByEmail(string email)
        {
            var allUser = await _userRepository.SearchByEmail(email);

            return _mapper.Map<List<UserDTO>>(allUser);
        }
        public async Task<UserDTO> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);

            return _mapper.Map<UserDTO>(user);
        }
    }
}